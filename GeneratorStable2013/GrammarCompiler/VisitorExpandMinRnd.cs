 
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.GrammarVisitors
{
  internal class VisitorExpandMinRnd : VisitorExpandBase
  {
    public override IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      AlternativeSet lMinAlt = aAlternativeSet.getMinUsageAlternatives();
      int i = mRnd.Next(lMinAlt.Count);
      return lMinAlt.GetAlternative(aContext, i);
    }
  }
}