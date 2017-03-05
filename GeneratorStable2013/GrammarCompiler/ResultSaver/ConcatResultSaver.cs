 
using System;

namespace GrammarCompiler.ResultSaver
{

  /// <summary>
  /// ResultSaver, ������� ��������� ��� � ���� ������, ����� �������� �� ����� �� ������.
  /// ������������ ��� ������������ �������� �� �������� ����������.
  /// </summary>
  public class ConcatResultSaver : IResultSaver
  {
    protected string mOutput;

    public virtual void Reset()
    {
      mCount = 0;
      mBadResults = 0;
      mTotalBadResults = 0;
      mOutput = "";
    }

    public virtual void Save(string aText)
    {
      ProcessTextEventArgs e = FireProcessText(aText);
      if (e.CancelSave) return;
      mOutput += e.Text;
      NewLine();
    }

    protected ProcessTextEventArgs FireProcessText(string aText)
    {
      ProcessTextEventArgs e = new ProcessTextEventArgs {Text = aText};
      if (ProcessText != null)
      {
        ProcessText(this, e);
        if (e.CancelSave)
        {
          MessageToLog("����� �� ������ �������� ������������: " + e.CancelReason);
          mBadResults++;
          mTotalBadResults++;
        }
        else
        {
          mCount++;
          mBadResults = 0;
        }
      }
      return e;
    }

    public void MessageToLog(string aText)
    {
      mOutput += aText;
      NewLine();
    }

    public virtual string GetFinalLog()
    {
      return mOutput;
    }

    private void NewLine()
    {
      mOutput += "\r\n";
    }

    public event EventHandler<ProcessTextEventArgs> ProcessText;

    private int mCount;
    private int mBadResults;
    private int mTotalBadResults;

    public int SavedCount
    {
      get { return mCount;  }
    }

    public int BadResults
    {
      get { return mBadResults; }
    }

    public int TotalBadResults
    {
      get { return mTotalBadResults; }
    }
  }
}