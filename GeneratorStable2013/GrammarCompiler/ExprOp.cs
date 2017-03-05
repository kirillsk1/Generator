 
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public enum eOperation
  {
    Add,
    Sub,
    Mul,
    Div
  }

  public interface IExpr : IPhrase
  {
    //float Eval(DerivationContext aContext)
    Type Type { get; }
    object GetValue(DerivationContext aContext);
  }

  public class ExprBase : IExpr, IDerivation
  {
    protected Type mType;

    public virtual Type Type
    {
      get { return mType; }
    }

    private IPhrase mParent;

    public IPhrase Parent
    {
      get { return mParent; }
      set { mParent = value; }
    }

    public virtual void PropagateCycle()
    {
    }

    #region IExpr Members

    public virtual object GetValue(DerivationContext aContext)
    {
      throw new NotImplementedException();
    }

    #endregion

    public virtual IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    public virtual TreeNode ToTree(TreeNode aParentNode)
    {
      throw new NotImplementedException();
    }

    public bool IsCyclic
    {
      get { return false; }
      set { throw new Exception("nafig"); }
    }

    public int UsageCount
    {
      get { throw new NotImplementedException(); }
    }

    public void IncUsageCount()
    {
      throw new NotImplementedException();
    }

    public string Alias
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }
  }


  public class ExprOp : ExprBase
  {
    public readonly List<IExpr> Operands;
    public ExprInt imResultAcc;
    public ExprDouble dmResultAcc;

    private readonly eOperation mOperation;

    public eOperation Operation
    {
      get { return mOperation; }
    }

    public override Type Type
    {
      get
      {
        DetermineResultType();
        return base.Type;
      }
    }

    public override object GetValue(DerivationContext aContext)
    {
      ExprBase lRes = (ExprBase) Accept(aContext);
      return lRes.GetValue(aContext);
    }

    public static eOperation ParseOperation(string aText)
    {
      if (aText == "+") return eOperation.Add;
      else if (aText == "-") return eOperation.Sub;
      else if (aText == "*") return eOperation.Mul;
      else if (aText == "/") return eOperation.Div;
      throw new ArgumentException("Unknown operation");
    }

    public ExprOp(eOperation aOperation)
    {
      mOperation = aOperation;
      Operands = new List<IExpr>();
    }


    public void DetermineResultType()
    {
      // if div or one of operands is double then double, else int
      if (mOperation == eOperation.Div)
      {
        mType = typeof (double);
      }
      else
      {
        mType = typeof (int);
        foreach (IExpr expr in Operands)
        {
          if (expr.Type == typeof (double))
          {
            mType = typeof (double);
            break;
          }
          else if (expr.Type == typeof (string))
          {
            mType = typeof (string);
            break;
          }
        }
      }
    }

    public void AccumulateResultd(double val)
    {
      double d = dmResultAcc.Value;
      switch (mOperation)
      {
        case eOperation.Add:
          d += val;
          break;
        case eOperation.Div:
          d /= val;
          break;
        case eOperation.Mul:
          d *= val;
          break;
        case eOperation.Sub:
          d -= val;
          break;
      }
      dmResultAcc.Value = d;
    }

    public void AccumulateResulti(int val)
    {
      int d = imResultAcc.Value;
      switch (mOperation)
      {
        case eOperation.Add:
          d += val;
          break;
        case eOperation.Div:
          d /= val;
          break;
        case eOperation.Mul:
          d *= val;
          break;
        case eOperation.Sub:
          d -= val;
          break;
      }
      imResultAcc.Value = d;
    }

    public override TreeNode ToTree(TreeNode aParentNode)
    {
      TreeNode lNode = aParentNode.Nodes.Add(mOperation.ToString());
      foreach (IExpr expr in Operands)
      {
        expr.ToTree(lNode);
      }
      return lNode;
    }

    public void AddOperand(IExpr expr)
    {
      Operands.Add(expr);
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }
}