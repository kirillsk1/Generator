 
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public class VisitorExpandRnd : VisitorExpandBase
  {
    public override IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      int i = mRnd.Next(aAlternativeSet.Count);
      return aAlternativeSet.GetAlternative(aContext, i);
    }
  }
}