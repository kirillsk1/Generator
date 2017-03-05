 
using System.Collections.Generic;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.Transforms
{
  /// <summary>
  /// Выполняется подстановка всего, что можно.
  /// </summary>    
  public class CollapseToOneBigRule : GrammarTransforms
  {
    private readonly List<string> mRulesToClear = new List<string>();

    protected override void DoTransform()
    {
      mRulesToClear.Clear();

      Rule main_rule = mGrammar.Rules[mGrammar.MainSymbol.Text];
      List<string> outerSymbols = new List<string>();
      outerSymbols.Add(main_rule.Name);
      main_rule.RightSide = Subst(main_rule.RightSide, outerSymbols);

      //Clear Rules
      foreach (string rule in mRulesToClear)
      {
        mGrammar.Rules.Remove(rule);
      }
    }


    private IPhrase Subst(NonTerminal symbol, List<string> aOuterSymbols)
    {
      //if (symbol.IsTerminal)
      //{
      //  if (!aOuterSymbols.Contains(symbol.Text))
      //  {
      //    mRulesToClear.Add(symbol.Text);
      //    List<string> outerSymbols = new List<string>(aOuterSymbols);     
      //    outerSymbols.Add(symbol.Text);
      //    return Subst(mGrammar.Rules[symbol.Text].RightSide, outerSymbols);
      //  }
      //}
      return symbol;
    }

    private IPhrase Subst(Seqence seqence, List<string> aOuterSymbols)
    {
      for (int i = 0; i < seqence.Phrases.Count; i++)
      {
        seqence.Phrases[i] = Subst(seqence.Phrases[i], aOuterSymbols);
      }
      return seqence;
    }

    private IPhrase Subst(AlternativeSet alt, List<string> aOuterSymbols)
    {
      for (int i = 0; i < alt.Phrases.Count; i++)
      {
        alt.Phrases[i] = Subst(alt.Phrases[i], aOuterSymbols);
      }
      return alt;
    }


    private IPhrase Subst(IPhrase iPhrase, List<string> aOuterSymbols)
    {
      return iPhrase;
    }
  }
}