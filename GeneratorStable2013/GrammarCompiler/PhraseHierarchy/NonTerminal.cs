 
using System;
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{    
  /// <summary>
  /// Представляет нетерминальный символ во внутреннем представлении правой части грамматики
  /// </summary>
  public class NonTerminal : IPhrase
  {
    private readonly NonTerminalCommon mCommon;
    private IPhrase mParent;
    public DerivationContext Context;
    //public int OccurenceInSeq; //какой он по счету в последовательности (2 для flower2)

    public string Text
    {
      get { return mCommon.mText; }
      //set { mText = value; }
    }

    public static implicit operator NonTerminalCommon(NonTerminal n)
    {
      return n.mCommon;
    }

    public string CounterName
    {
      get { return Text + OccurenceId; }
    }

    /// <summary>
    /// Номер вхождения в грамматике
    /// </summary>
    public int OccurenceId;

    public static NonTerminal Create(Grammar aGrammar, IPhrase aParent, string aText)
    {
      //for terminal we don't return existing object, because it may be modified (terminal)
      bool lAllreadyExists = aGrammar.AllNonTerminals.ContainsKey(aText);
      NonTerminalCommon common;
      if (lAllreadyExists)
      {
        common = aGrammar.AllNonTerminals[aText];
      }
      else
      {
        common = new NonTerminalCommon(aGrammar, aText);
        aGrammar.AllNonTerminals.Add(aText, common);
      }
      NonTerminal lSym = new NonTerminal(common, aParent);
      lSym.OccurenceId = common.AllOccurences.Count;
      common.AllOccurences.Add(lSym);
      return lSym;
    }

    protected NonTerminal(NonTerminalCommon aCommon, IPhrase aParent)
    {
      mCommon = aCommon;
      mParent = aParent;
    }


    public NonTerminal(NonTerminal aSymbol)
    {
      mCommon = aSymbol.mCommon;
      mParent = aSymbol.Parent;
    }

    public virtual TreeNode ToTree(TreeNode aNode)
    {
      return aNode.Nodes.Add(GetType().Name);
    }

    public virtual IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    public override string ToString()
    {
      return Text;
    }

    public Grammar Grammar
    {
      get { return mCommon.Grammar; }
    }

    public CycicKind CycicKind = CycicKind.None;
    public string CyclicToOccurence;

    #region Члены IPhrase

    public bool IsCyclic { get; set; }

    public virtual void PropagateCycle()
    {
      CycicKind |= CycicKind.CyclicPropagated;
      IsCyclic = true;
      if (Parent != null)
      {
        Parent.PropagateCycle();
      }
      else
      {
        //MessageBox.Show(string.Format("Символ {0} сирота", Text));
      }
    }

    public int UsageCount
    {
      get { return mCommon.UsageCount; }
    }

    public void IncUsageCount()
    {
      mCommon.IncUsageCount();
    }

    public string Alias
    {
      get { return mCommon.Alias; }
      set { mCommon.Alias = value; }
    }

    public IPhrase Parent
    {
      get { return mParent; }
      set { mParent = value; }
    }

    #endregion

    /// <summary>
    /// Возвращает правило, соответствующее данному нетерминалу.
    /// Исключения:
    ///   GrammarDeductException "Не определен нетерминальный символ {0}"
    /// </summary>
    /// <returns>гарантированно не null</returns>
    public Rule FindItsRule()
    {
      // искать правило
      if (Grammar.Rules.ContainsKey(Text))
      {
        //взали правило
        return Grammar.Rules[Text];        
      }
      // нашли нетерм. символ, который нельзя раскрыть
      throw new GrammarDeductException(string.Format("Не определен нетерминальный символ {0}", Text));
    }
  }
}