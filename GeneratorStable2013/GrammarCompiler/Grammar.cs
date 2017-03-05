 
//   ��������:
//     ����������� ������ - ��������� �������� ����������
//
//  ����: 25.12.2007
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;
using GrammarTransductors;
using GraphTools;
using VisualStructure;

namespace GrammarCompiler
{
  public class Grammar
  {
    public readonly Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
    public readonly Dictionary<string, NonTerminalCommon> AllNonTerminals = new Dictionary<string, NonTerminalCommon>();
    public readonly List<NonTerminal> AllCyclicSymbolOccurs = new List<NonTerminal>();
    public string[] GrammarText;
    private NonTerminal mMainSymbol;
    public SysTransductor SysTrans = new SysTransductor();
    public ListTrans ListTrans = new ListTrans();
    public List<GrammarSyntaxException> SyntaxErrors = new List<GrammarSyntaxException>();
    public string LexerDebugText;
    //public DictionaryDerivation RuleProtocol;
    private List<string> mMainSymbolCandidats;

    public static SpecificGraphBuilder SpecGraphBuilder;
    public static GraphBuilder GraphBuilder;

    public tGraph GrammarGraph;
    
    public NonTerminal MainSymbol
    {
      get { return mMainSymbol; }
    }

    #region constructors, start parsing

    public static Grammar FromText(string aText)
    {
      MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(aText));
      return new Grammar(new StreamReader(ms));
    }

    public static Grammar FromTextFile(string aFileName)
    {
      return new Grammar(new StreamReader(aFileName));
    }

    public Grammar(StreamReader aStream)
    {
      ParseInit();
      while (!aStream.EndOfStream)
      {
        string lLine = aStream.ReadLine();
        ParseLine(lLine);
      }
      ParseFinish();
    }

    public Grammar(string[] aGrammarText)
    {
      GrammarText = aGrammarText;
      ParseInit();
      foreach (string s in aGrammarText)
      {
        ParseLine(s);
      }
      ParseFinish();
    }

    #endregion

    #region Parsing grammar

    private int mLine;
    private string mPrevS;

    private void ParseInit()
    {
      LexerDebugText = "";
      AllNonTerminals.Clear();
      Rules.Clear();
      mLine = 0;
      mPrevS = "";
    }

    private void ParseLine(string aLine)
    {
      string lInputLine = aLine.Trim();
      //������� ����������� � ����� ������
      int liComment = lInputLine.IndexOf("//");
      if (liComment >= 0)
      {
        lInputLine = lInputLine.Substring(0, liComment);
      }
      // ���� ������ ������������ \, �� ���������� �� �� ���������
      if (lInputLine.EndsWith(@"\"))
      {
        mPrevS += lInputLine.Substring(0, lInputLine.Length - 1);
      }
      else
      {
        lInputLine = mPrevS + lInputLine;
        mPrevS = "";
        // ������� �������.
        if (!string.IsNullOrEmpty(lInputLine))
        {
          Lexer lexer = new Lexer(lInputLine);
          Lexema lex = lexer.GetNext();
          try
          {
            Rule lRull = new Rule(this, lex);
            // ���������, ���� ���� ����� ������� � ���� ������, �� �� ��������� �������, � ��������� ������������
            if (Rules.ContainsKey(lRull.Name))
            {
              throw new GrammarSyntaxException("������� � ���� ������ ��� ����������!", aLine);
              //���������� �������
              //Rules[lRull.mLeftSide].mRightSides.mAlternatives.AddRange(lRull.mRightSides.mAlternatives);
            }
            else
            {
              Rules.Add(lRull.Name, lRull);
            }
          }
          catch (GrammarSyntaxException ex)
          {
            ex.Line = mLine;
            ex.Col = lexer.Col;
            ex.LineText = lInputLine;
            SyntaxErrors.Add(ex);
          }
          finally
          {
            LexerDebugText += lexer.DebugText;
          }
        }
      }
      mLine++;
    }

    private void ParseFinish()
    {
      try
      {
        FindMainSymbol();
        FindCycles();
        BuildGrammarGraph();
        if (null != GraphBuilder)
        {
          DrawGrammarGraph();
          //DrawAnyGraph(mCycleGraph);
        }        
      }
      catch (GrammarSyntaxException ex)
      {
        SyntaxErrors.Add(ex);
      }
      catch (GrammarDeductException ex)
      {
        SyntaxErrors.Add(new GrammarSyntaxException(ex.Message));
      }
    }

    public override string ToString()
    {
      string result = "";
      result += string.Format(
        @"//
// MainSymbol: {0}
// Rules: {1}
// AllNotTerminals: {2}
//

", mMainSymbol, Rules.Count,
        AllNonTerminals.Count);
      foreach (Rule rule in Rules.Values)
      {
        result += rule + "\r\n";
      }
      return result;
    }

    public void ToTree(TreeNodeCollection aNodes)
    {
      foreach (KeyValuePair<string, Rule> lRull in Rules)
      {
        TreeNode lNode = new TreeNode(lRull.Key);
        aNodes.Add(lNode);
        lRull.Value.RightSide.ToTree(lNode);
      }
    }
    
    #endregion

    private void FindMainSymbol()
    {
      mMainSymbolCandidats = new List<string>();
      string mainSymbolDefault = "";
      foreach (String lRuleName in Rules.Keys)
      {
        if (!AllNonTerminals.ContainsKey(lRuleName))
        {
          mMainSymbolCandidats.Add(lRuleName);
        }
        if (lRuleName[0] == '_')
        {
          mainSymbolDefault = lRuleName;
        }
      }
      if (mMainSymbolCandidats.Count == 0)
      {
        throw new GrammarSyntaxException("��� �������� �������");
      }
      else if (mMainSymbolCandidats.Count > 1 && mainSymbolDefault == "")
      {
        string lNotDefSyms = string.Join(", ", mMainSymbolCandidats.ToArray());
        throw new GrammarSyntaxException(lNotDefSyms +
                                         " - ������� ��������� �������������� �������������� �������� - ���������� �� ���� ��������");
      }
      else if (mainSymbolDefault != "")
      {
        mMainSymbol = NonTerminal.Create(this, null, mainSymbolDefault);
      }
      else
      {
        mMainSymbol = NonTerminal.Create(this, null, mMainSymbolCandidats[0]);
      }
    }

    /// <summary>
    /// ���������� ����������� ������� � ������������
    /// </summary>
    public void FindCycles()
    {
      // ��������
      AllCyclicSymbolOccurs.Clear();
      foreach (NonTerminalCommon curSymbol in AllNonTerminals.Values)
      {
        curSymbol.IsCyclic = false;
      }
      
      // 1) ������ ������ - ���������� ������ ����������� �������
      mMainSymbol.Context = new CyclesDetectContext(this);
      mMainSymbol.Context.DerivPath.Add(mMainSymbol.Text);
      mMainSymbol.Context.Visitor = new CyclesDetectVisitor();
      //��
      //context.BuildingGraph = new tGraph();
      //context.BuildingGraph.addNode(MainSymbol.CounterName);
      
      mMainSymbol.Accept(mMainSymbol.Context);

      // 2) ������ �� �������� ��������� ��� �����
      //foreach (NonTerminalCommon cyclicCommon in AllCyclicSymbols)
      //{
      //  foreach (NonTerminal nonTerminal in cyclicCommon.AllOccurences)
      //  {
      //    nonTerminal.Parent.PropagateCycle();
      //  }
      //}

      // 3) ����������� - ������ ���� ������ ���� ������������� ������������
      DerivationContext context = new DerivationContext(this);
      CheckCyclesVisitor checkVisitor = new CheckCyclesVisitor();
      context.Visitor = checkVisitor;
      foreach (Rule rule in Rules.Values)
      {
        rule.RightSide.Accept(context);
      }
      if (!string.IsNullOrEmpty(checkVisitor.Log))
      {
        //���� ������
        MessageBox.Show(checkVisitor.Log);
      }
    }

    /// <summary>
    /// ������ ���� ����������. ��� ���������� �����������
    /// </summary>
    public void BuildGrammarGraph()
    {
      GrammarGraph = new tGraph();
      // ������ ������� ����� ����������
      //foreach (string lNotTermName in Rules.Keys) //AllNotTerminals.Keys) ��� ������, ����� ���� �������������� �������
      //{
      //  GrammarGraph.addNode(lNotTermName);
      //}
      GrammarGraph.addNode(MainSymbol.Text);

      BuildGraphContext context = new BuildGraphContext(this, GrammarGraph);
      mMainSymbol.Context = context;
      mMainSymbol.Context.Visitor = new BuildGraphVisitor();
      mMainSymbol.Accept(context);
    }

    public void DrawGrammarGraph()
    {
      GraphBuilder.Start();
      foreach (tEdge ed in GrammarGraph.Edges)
      {
        GrammarGraphEdgeAttrs attr = (GrammarGraphEdgeAttrs)ed.CustomAttributes;
        if ((attr & GrammarGraphEdgeAttrs.IsSequence) > 0)
        {
          GraphBuilder.AddDoubleEdge(ed.fromNode, ed.toNode);
        }
        else if ((attr & GrammarGraphEdgeAttrs.IsCyclic) > 0)
        {
          GraphBuilder.AddTribbleEdge(ed.fromNode, ed.toNode);
        }
        else if ((attr & GrammarGraphEdgeAttrs.IsPunktir) > 0)
        {
          GraphBuilder.AddPunktirEdge(ed.fromNode, ed.toNode);
        }
        else
        {
          GraphBuilder.AddSimpleEdge(ed.fromNode, ed.toNode);
        }
      }

      foreach (tNode node in GrammarGraph.Nodes.Values)
      {
        if ((string)node.CustomAttributes == "term")
        {
          GraphBuilder.SetBGColor(node.Name, 200, 255, 200);
        }
      }
      GraphBuilder.SetBGColor(mMainSymbol.Text, 225, 41, 25);
      GraphBuilder.SetBGColor(mMainSymbol.CounterName, 225, 41, 25);
      GraphBuilder.End();
    }

    //����� �������� ������������ ����, �������� ���� ������, � ��������, ��� �������
    public void DrawAnyGraph(tGraph aGraph)
    {
      GraphBuilder.Start();
      foreach (tEdge ed in aGraph.Edges)
      {
        GraphBuilder.AddSimpleEdge(ed.fromNode, ed.toNode);
      }
      GraphBuilder.End();
    }
  }
}