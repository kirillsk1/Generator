 
using System;
namespace GrammarCompiler.ResultSaver
{
  /// <summary>
  /// ��������� ��������������� �����
  /// 
  /// ������������ ��� ���������� �������� � ��������� ����� (����� Save)
  /// � ����� ����� ��� ���������
  /// </summary>
  public interface IResultSaver
  {
    /// <summary>
    /// �����. ������� ���
    /// </summary>
    void Reset();
    /// <summary>
    /// ����������� � ��������� ��������������� �����
    /// </summary>
    /// <param name="aText">�����</param>
    void Save(string aText);
    /// <summary>
    /// ��������� ��������� � ��� (� ������� ������).
    /// </summary>
    /// <param name="aText">���������</param>
    void MessageToLog(string aText);
    //���������� ������� ���
    string GetFinalLog();
    /// <summary>
    /// ���������� � ������ Save ����� ����������� ������. ���������� ����� ������� ����� ������� (��������������� � �.�.) ����� ��� �������� ��� ����������.
    /// </summary>
    event EventHandler<ProcessTextEventArgs> ProcessText;

    /// <summary>
    /// ���������� ����������� ������� (������������� MinGW �� �����������)
    /// </summary>
    int SavedCount { get; }

    /// <summary>
    /// ������� ������� ������ ���� ���������� ������������
    /// </summary>
    int BadResults { get; }
    
    /// <summary>
    /// ������� ������� ����� ���� ���������� ������������
    /// </summary>
    int TotalBadResults { get; }    
  }
}