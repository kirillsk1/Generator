using System;
namespace GrammarCompiler.ResultSaver
{
  public class ProcessTextEventArgs : EventArgs
  {
    /// <summary>
    /// ��������������� ����� ������� ����� ����������
    /// </summary>
    public string Text;
    /// <summary>
    /// ���������, ��� �� ���� ��������� ���� ����� (��������, ����� �� �� ������ �������� �� MinGW)
    /// </summary>
    public bool CancelSave;

    /// <summary>
    /// �������� ������ ��-�� ������� ���������� ��������� CancelSave=true
    /// </summary>
    public string CancelReason;
  }
}