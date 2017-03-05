using System;
namespace GrammarCompiler
{
  public class GenerateProgressEventArgs : EventArgs
  {
    public string RootText;
    public string CurrentListText;
    public int CurrentListLevel;
    public int CurrentInRoot;
    public int TotalInRoot;
    public int CurrentInCurrent;
    public int MaxLevel;
    /// <summary>
    /// ���� �� null, ������ ��������� ����������� �� ����� �� ���� ��������
    /// ����� back trace
    /// </summary>
    public ListDerivation PausedAtList;
  }
}