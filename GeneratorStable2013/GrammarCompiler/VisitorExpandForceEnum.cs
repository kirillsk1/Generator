 
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public class VisitorExpandForceEnum : VisitorExpandBase
  {
    public override IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      //1. Взять контекстный счетчик
      //1.1 Найти в словаре, если нет, создать
      Counter lCounter =
        Generator.CounterDictionary.FindOrAddCounter(
          aContext.DervPathWithOccurAsString + "__" + aContext.ExpandingRuleSymbol.CounterName, aAlternativeSet.Count);
      bool stopGen = aContext.Generator.StopGenerate;
      if (aContext.Generator.Options.AlternativeAlg != AlternativeSelectAlg.Enum)
      aContext.Generator.StopGenerate = false;
      IDerivation lResult = aAlternativeSet.GetAlternative(aContext, lCounter.Value);
      aContext.Generator.StopGenerate = stopGen;
      return lResult;
    }

    //public override IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext)
    //{
    //  //TODO 11111111!!!!!!!!
    //  throw new System.NotImplementedException();
    //}
  }
}