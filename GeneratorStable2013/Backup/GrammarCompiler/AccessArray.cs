 
using System;
using System.Windows.Forms;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public class Access : PhraseBase, IExpr
  {
    public readonly string ObjectName;
    public IExpr mParentAccess;

    public Access(Grammar aGrammar, string aName) : base(aGrammar)
    {
      ObjectName = aName;
    }

    public Access(Grammar aGrammar, IExpr aParentAcc) : base(aGrammar)
    {
      mParentAccess = aParentAcc;
    }

    public Type Type
    {
      get { return typeof (IDerivation); }
    }

    #region IExpr Members

    public object GetValue(DerivationContext aContext)
    {
      return Accept(aContext);
    }

    #endregion

    public override TreeNode ToTree(TreeNode aParentNode)
    {
      return aParentNode.Nodes.Add("Access to " + ObjectName);
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }

  public class AccessArray : Access
  {
    public IExpr IndexExpr;

    public AccessArray(Grammar aGrammar, Access aParentAcc, IExpr aInd) : base(aGrammar, aParentAcc)
    {
      IndexExpr = aInd;
    }

    public override string ToString()
    {
      string parent = "";
      if (mParentAccess != null)
      {
        parent = mParentAccess.ToString();
      }
      return parent + "[" + IndexExpr.ToString() + "]";
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }

  public class AccessSeq : Access
  {
    public string mFieldName;

    public AccessSeq(Grammar aGrammar, IExpr aParentAcc, string aFieldName)
      : base(aGrammar, aParentAcc)
    {
      mFieldName = aFieldName;
    }

    public override string ToString()
    {
      string parent = "";
      if (mParentAccess != null)
      {
        parent = mParentAccess.ToString();
      }
      return parent + "." + mFieldName;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }
}