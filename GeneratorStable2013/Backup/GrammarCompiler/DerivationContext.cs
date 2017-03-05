 
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public class DerivationContextBase
  {
    public Grammar Grammar;
    public IVisitor Visitor;

    public DerivationContextBase(Grammar aGrammar)
    {
      Grammar = aGrammar;
    }
  }

  public class DerivationContext : DerivationContextBase
  {
    #region Fields

    public Generator Generator;
    public TreeNode RuleDerivNode;
    public NonTerminal ExpandingRuleSymbol;
    //public int LevelCount; was not used yet
    public ProtocolArray RuleSeqProtocol;
    public DerivationBase ParentDerivation;
    private List<string> mDerivPath;
    public XmlDocument DerivationXml;
    public XmlNode currentNode;

    #endregion

    public DerivationContext(Grammar aGrammar)
      : base(aGrammar)
    {
      DerivationXml = new XmlDocument();
      currentNode = DerivationXml.CreateElement(aGrammar.MainSymbol.Text);
      DerivationXml.AppendChild(currentNode);
    }

    public DerivationContext(DerivationContext c) : base(c.Grammar)
    {
      Generator = c.Generator;
      RuleDerivNode = c.RuleDerivNode;
      ExpandingRuleSymbol = c.ExpandingRuleSymbol;
      //LevelCount = c.LevelCount;
      RuleSeqProtocol = c.RuleSeqProtocol;
      ParentDerivation = c.ParentDerivation;
      Visitor = c.Visitor;
      DerivationXml = c.DerivationXml;
      currentNode = c.currentNode;
    }

    #region Readonly Properties

    public string ExpandingRuleName
    {
      get { return ExpandingRuleSymbol.Text; }
    }

    public List<string> DerivPath
    {
      get
      {
        if (null == mDerivPath)
        {
          mDerivPath = new List<string>();
          TreeNode lNode = RuleDerivNode;
          while (lNode != null)
          {
            mDerivPath.Insert(0, lNode.Text);
            lNode = lNode.Parent;
          }
        }
        return mDerivPath;
      }
    }

    public int LevelCount
    {
      get { return DerivPath.Count; }
    }

    public List<string> DerivPathWithOccur
    {
      get
      {
        List<string> lDerivPath = new List<string>();
        TreeNode lNode = RuleDerivNode;
        while (lNode != null)
        {
          lDerivPath.Insert(0, lNode.Text + lNode.Tag);
          lNode = lNode.Parent;
        }
        return lDerivPath;
      }
    }

    public string DervPathWithOccurAsString
    {
      get
      {
        List<string> lDerivPath = DerivPathWithOccur;
        return string.Join("//", lDerivPath.ToArray());
      }
    }

    #endregion
  }
}