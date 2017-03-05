 
using GrammarCompiler.PhraseHierarchy;
using Tools;

namespace GrammarCompiler
{
  public class VisitorExpandNormRnd : VisitorExpandRnd
  {
    public override IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      int i = 0;
      NormalDistribution lNormRnd = new NormalDistribution(0, aAlternativeSet.Count/2);
      i = lNormRnd.intND();
      if (i < 0 || i >= aAlternativeSet.Count) i = 0;
      return aAlternativeSet.GetAlternative(aContext, i);
    }
  }
}