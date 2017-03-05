 
using System.Windows.Forms;

namespace GrammarCompiler
{
  public class ExprValue<T> : ExprBase
  {
    protected T mValue;

    public virtual T Value
    {
      get { return mValue; }
      set { mValue = value; }
    }

    public static implicit operator T(ExprValue<T> pv)
    {
      return pv.Value;
    }

    public ExprValue()
    {
      mType = typeof (T);
    }

    public ExprValue(T aVal)
    {
      mValue = aVal;
    }

    public override object GetValue(DerivationContext aContext)
    {
      return mValue;
    }

    public override string ToString()
    {
      return mValue.ToString();
    }

    public override TreeNode ToTree(TreeNode aParentNode)
    {
      return aParentNode.Nodes.Add(Value.ToString());
    }
  }

  public class ExprDouble : ExprValue<double>
  {
    public ExprDouble()
    {
    }

    public ExprDouble(double aVal)
    {
      mValue = aVal;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }

  public class ExprInt : ExprValue<int>
  {
    public ExprInt()
    {
    }

    public ExprInt(int aVal)
    {
      mValue = aVal;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }

  public class ExprString : ExprValue<string>
  {
    public ExprString(string aVal)
    {
      mValue = aVal;
    }
  }

  public class ExprEval<T> : ExprValue<T>
  {
  }

  //TODO: review conf vars
  public class ExprConfVariable : ExprDouble
  {
    public string Name;
    //public Context
    public ExprConfVariable(string aVarName) : base(0)
    {
      Name = aVarName;
    }

    public override double Value
    {
      get
      {
        //TODO read from config xml by context and Name
        object o1 = 5;
        return (double) o1;
      }
    }
  }
}