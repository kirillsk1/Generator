 
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет во внутреннем представлении правой части грамматики
  /// Команду вызова трансдуктора
  /// Например,   
  /// b := Rnd(90,120)
  /// </summary>
  public class TransCallPhrase : PhraseBase, IExpr
  {
    private readonly string mName;
    public readonly List<IExpr> Params;
    public MethodInfo BindedMethod = null;
    public readonly object TransductorClass;
    public readonly Access TargetAccess;

    public TransCallPhrase(Grammar aGrammar, string aName) : base(aGrammar)
    {
      mName = aName;
      Params = new List<IExpr>();
    }

    public TransCallPhrase(Grammar aGrammar, string aName, Access aTargetAccess)
      : this(aGrammar, aName)
    {
      TargetAccess = aTargetAccess;
      if (TargetAccess == null)
      {
        TransductorClass = Grammar.SysTrans;
      }
      else
      {
        TransductorClass = Grammar.ListTrans;
      }
    }

    public override string ToString()
    {
      string result = "";
      if (TargetAccess != null)
      {
        result += TargetAccess.ToString() + ".";
      }
      result += mName + "(";
      result += ParamsToString();
      result += ")";
      return result;
    }

    public string ParamsToString()
    {
      string pars = "";
      foreach (IExpr expr in Params)
      {
        if (pars != "") pars += ", ";
        pars += expr.ToString();
      }
      return pars;
    }

    #region IExpr Members

    public Type Type
    {
      get
      {
        if (BindedMethod.ReturnType == typeof (IDerivation))
        {
          return typeof (string);
        }
        return BindedMethod.ReturnType;
      }
    }

    public object GetValue(DerivationContext aContext)
    {
      return Accept(aContext);
    }

    #endregion

    public void DoBinding()
    {
      Type lTType = TransductorClass.GetType();
      MethodInfo[] mi = lTType.GetMethods();
      bool lIsNameMatched = false;
      bool ParamsOKFlag = false;
      string lParamMismatchMsg = "";
      foreach (MethodInfo info in mi)
      {
        if (info.Name == mName)
        {
          lIsNameMatched = true;
          //chek parametrs
          ParameterInfo[] parametrs = info.GetParameters();
          if (parametrs.Length == Params.Count)
          {
            ParamsOKFlag = true;
            int i = 0;
            foreach (IExpr TransParam in Params)
            {
              if (parametrs[i].ParameterType != TransParam.Type)
              {
                ParamsOKFlag = false;
                lParamMismatchMsg = string.Format("Can't convert {0} to {1}", TransParam.Type,
                                                  parametrs[i].ParameterType);
                break;
              }
              i++;
            }
            if (ParamsOKFlag)
            {
              BindedMethod = info;
              break;
            }
          }
        }
      }
      //error checking
      if (!lIsNameMatched)
      {
        throw new GrammarSyntaxException(string.Format("No Transductor '{0}'", mName));
      }
      else if (!ParamsOKFlag)
      {
        throw new GrammarSyntaxException(string.Format("Transductor '{0}' parameters type mismatch. {1}", mName,
                                                       lParamMismatchMsg));
      }
    }

    /*public void AddParametr(ExprDouble number)
    {
      TransParam lPar = new TransParam(number);
      lPar.Type = typeof (double);
      lPar.IsVariable = number is ExprConfVariable;
      AddParametr(lPar);
    }
    public void AddParametr(TextDerivation aTextDeriv)
    {
      TransParam lPar = new TransParam(aTextDeriv);
      lPar.Type = typeof(string);
      lPar.IsVariable = false;//TODO: SUX
      AddParametr(lPar);
    }*/

    public void AddParametr(IExpr value)
    {
      Params.Add(value);
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }

  /*  public class TransParam
  {
    public Type Type;
    public bool IsVariable;
    private readonly IDerivation mParam;

    public TransParam(IDerivation param)
    {
      mParam = param;
    }

    public object Value
    {
      get
      {
        if (mParam is ExprDouble)
        {
          return ((ExprDouble) mParam).Value;
        }
        else if (mParam is TextDerivation)
        {
          return ((TextDerivation) mParam).ToString();
        }
        throw new Exception("unknown param type");
      }
    }
  }*/
}