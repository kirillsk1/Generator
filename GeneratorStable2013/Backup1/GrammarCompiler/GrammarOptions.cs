 
namespace GrammarCompiler
{
  public enum AlternativeSelectAlg
  {
    /// <summary>
    /// Usual rnd
    /// </summary>
    RndDistr,
    /// <summary>
    /// Normal distribution around first alternative
    /// </summary>
    NormalDistr,
    /// <summary>
    /// Perebor )
    /// </summary>
    Enum,
    /// <summary>
    /// All pairs enum
    /// </summary>
    Pairs,
    /// <summary>
    /// Special alg. selects alternative with min usage count
    /// </summary>
    MinRnd
    /// <summary>
    /// Special alg. selects dugi
    /// </summary>
  }

  public class GrammarOptions
  {
    public AlternativeSelectAlg AlternativeAlg = AlternativeSelectAlg.NormalDistr;

    /// <summary>
    /// ���� ������ ������� � ��� �����, ��� �������� ������� � ���� ���� (����� ������ ��������� � ��������� ����)
    /// </summary>
    /// // todo: get options
    public bool AllEnumInOne = true;

    public bool RREnable = true;

    /// <summary>
    /// ������� ������� ����� �������������
    /// </summary>
    public int TestsAmount = 1;

    public int LevelRestriction = 7;
  }
}