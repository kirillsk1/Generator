 
using System.Collections.Generic;
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет Набор Альтернатив во внутреннем представлении правой части грамматики
  /// Например, 
  /// S := A | B
  /// </summary>  
  public class AlternativeSet : PhraseList
  {
    public AlternativeSet(Grammar aGrammar, IPhrase aPhr)
      : base(aGrammar, aPhr)
    {
    }

    public AlternativeSet(Grammar aGrammar)
      : base(aGrammar)
    {
    }

    public AlternativeSet getMinUsageAlternatives()
    {
      AlternativeSet minAlternative = new AlternativeSet(Grammar);
      int minUsage = int.MaxValue;
      foreach (IPhrase curPhrase in Phrases)
      {
        if (curPhrase.UsageCount < minUsage)
        {
          minUsage = curPhrase.UsageCount;
        }
      }
      foreach (IPhrase curPhrase in Phrases)
      {
        if (curPhrase.UsageCount == minUsage)
        {
          minAlternative.Phrases.Add(curPhrase);
        }
      }
      return minAlternative;
    }

    public IDerivation GetAlternative(DerivationContext aContext, int i)
    {
      if (aContext.Generator.StopGenerate)
      {
        List<IPhrase> NotCyclic = new List<IPhrase>();
        foreach (IPhrase p in mPhrases)
        {
          if (!p.IsCyclic)
          {
            NotCyclic.Add(p);
          }
        }
        if (NotCyclic.Count == 0)
        {
          throw new GrammarDeductException("При попытке ограничения уровня влож. Все альтернативы циклические.");
        }
        i = i%NotCyclic.Count; //Ограничили
        return NotCyclic[i].Accept(aContext);
      }
      else
      {
        mPhrases[i].IncUsageCount();
        return mPhrases[i].Accept(aContext);
      }
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    //public override bool IsCyclic(string aRuleName)
    //{
    //  {
    //    bool cyclic = true;
    //    foreach (IPhrase phr in mPhrases)
    //    {
    //      cyclic &= phr.IsCyclic(aRuleName);
    //    }
    //    return cyclic;
    //  }
    //}

    public override string ToString()
    {
      return ToString(true);
    }

    public string ToString(bool new_line)
    {
      string result = "";
      foreach (IPhrase phrase in mPhrases)
      {
        if (result != "")
        {
          if (new_line) result += "\\\r\n\t";
          result += " | ";
        }
        result += phrase.ToString();
      }
      return result;
    }

    public override void PropagateCycle()
    {
      foreach (IPhrase phrase in Phrases)
      {
        if (!phrase.IsCyclic) return;
      }
      MessageBox.Show("Альтернатива " + this + " уже нажралась циклов!");
      base.PropagateCycle();
    }
  }
}