 
using System;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет Квантификатор во внутреннем представлении правой части грамматики
  /// Например, 
  /// S := A{2..5}
  /// </summary>  
  public class QuantifiedPhrase : PhraseBase
  {
    public IPhrase Phrase;
    public ExprInt Min;
    public ExprInt Max;
    public bool IsConfigurable;

    #region Constructors

    public QuantifiedPhrase(Grammar aGrammar, IPhrase aPhr, ExprInt aMin, ExprInt aMax)
      : base(aGrammar)
    {
      Phrase = aPhr;
      Min = aMin;
      Max = aMax;
      IsConfigurable = false;
    }

    public QuantifiedPhrase(Grammar aGrammar, IPhrase aPhr)
      : this(aGrammar, aPhr, 0, int.MaxValue)
    {
      IsConfigurable = true;
    }

    public QuantifiedPhrase(Grammar aGrammar, IPhrase aPhr, ExprInt aMinMax)
      : this(aGrammar, aPhr, aMinMax, aMinMax)
    {
    }

    public QuantifiedPhrase(Grammar aGrammar, IPhrase aPhr, int aMin, int aMax)
      : this(aGrammar, aPhr, new ExprInt(aMin), new ExprInt(aMax))
    {
    }

    public QuantifiedPhrase(Grammar aGrammar, IPhrase aPhr, int aMinMax)
      : this(aGrammar, aPhr, new ExprInt(aMinMax))
    {
    }

    #endregion

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    public string QuantSign
    {
      get
      {
        if (Min == 0 && Max == 1)
        {
          return "?";
        }
        else if (Max == Int32.MaxValue)
        {
          if (Min == 0)
          {
            return "*";
          }
          else if (Min == 1)
          {
            return "+";
          }
          else
          {
            return "{" + Min + "..*}";
          }
        }
        else if (Min == Max)
        {
          return "{" + Min + "}";
        }
        {
          return "{" + Min + ".." + Max + "}";
        }
      }
    }

    public override string ToString()
    {
      string phr = Phrase.ToString();
      if (Min == 0 && Max == 1)
      {
        return string.Format("[ {0} ]", phr);
      }
      else if (Phrase is NonTerminal || Phrase is Terminal)
      {
        return string.Format("{0}{1}", phr, QuantSign);
      }
      else
      {
        return string.Format("({0}){1}", phr, QuantSign);
      }
    }

    public override void PropagateCycle()
    {
      //IsCyclic = false;
    }
  }
}