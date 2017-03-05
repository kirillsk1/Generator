 
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.GrammarVisitors
{
  /// <summary>
  /// Верификация выявленных циклических альтернатив, заключающаяся в том, что в каждом наборе альтернатив должна быть хотя бы одна НЕ циклическая альтернатива. 
  /// Противное будет означать ошибку в грамматике либо определениях циклического символа и альтернативы (или в реализации алгоритма).
  /// </summary>
  public class CheckCyclesVisitor : WalkVisitorBase
  {
    public string Log = "";

    public override IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      bool allIsCyclic = true;
      // allIsCyclic сохранит значение true в том и только том случае, если все логические сомножетели true
      for (int i = 0; i < aAlternativeSet.Count && allIsCyclic; i++)
      {
        //allIsCyclic = allIsCyclic & aAlternativeSet.Phrases[i].IsCyclic;
        allIsCyclic &= aAlternativeSet.Phrases[i].IsCyclic;
      }
      if (allIsCyclic)
      {
        IPhrase p = aAlternativeSet.Parent;
        while (p != null && ! (p is NonTerminal))
        {
          p = p.Parent;
        }
        string ruleName = (p != null) ? p.ToString() : "main";
        Log += string.Format("В правиле {0} набор альтернатив {1} ошибка - все альтернативы циклические.\r\n", ruleName,
                             aAlternativeSet);
      }
      return base.Visit(aAlternativeSet, aContext);
    }
  }
}