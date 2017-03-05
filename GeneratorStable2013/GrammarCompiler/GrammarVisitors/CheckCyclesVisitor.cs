 
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.GrammarVisitors
{
  /// <summary>
  /// ����������� ���������� ����������� �����������, ������������� � ���, ��� � ������ ������ ����������� ������ ���� ���� �� ���� �� ����������� ������������. 
  /// ��������� ����� �������� ������ � ���������� ���� ������������ ������������ ������� � ������������ (��� � ���������� ���������).
  /// </summary>
  public class CheckCyclesVisitor : WalkVisitorBase
  {
    public string Log = "";

    public override IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      bool allIsCyclic = true;
      // allIsCyclic �������� �������� true � ��� � ������ ��� ������, ���� ��� ���������� ����������� true
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
        Log += string.Format("� ������� {0} ����� ����������� {1} ������ - ��� ������������ �����������.\r\n", ruleName,
                             aAlternativeSet);
      }
      return base.Visit(aAlternativeSet, aContext);
    }
  }
}