 
using System;
using System.IO;
using System.Text;

namespace GrammarCompiler.ResultSaver
{
  /// <summary>
  /// Настоящий ResultSaver - записывает программы в файлы.
  /// Также ведет отдельный лог в который записываются сообщения от перебирающего алгоритма
  /// и в конце этот лог показывается на экране
  /// 
  /// Лог наследуется от <see cref="ConcatResultSaver"/>
  /// </summary>
  public class FileResultSaver : ConcatResultSaver //IResultSaver
  {
    string TargetFolder;    
    private int mTotalTests;
    private string subDirName;

    public FileResultSaver(string aTargetFolder, string aLabel, int aTotalTests)
    {
      mTotalTests = aTotalTests;
      if (!Directory.Exists(aTargetFolder))
      {        
        Directory.CreateDirectory(aTargetFolder);
      }
      subDirName = aLabel + "_"; //+ DateTime.Now.ToString("hh-mm-ss-dd-MM-yyyy");
      TargetFolder = Path.Combine(aTargetFolder, subDirName);
      if (!Directory.Exists(TargetFolder))
      {
        Directory.CreateDirectory(TargetFolder);
      }
    }

    public override void Save(string aText)
    {
      ProcessTextEventArgs e = FireProcessText(aText);
      if (e.CancelSave) return;
      
      using (StreamWriter curFile =
        File.CreateText(Path.Combine(TargetFolder, SetFileName(SavedCount)) + ".c"))
      {
        curFile.WriteLine(e.Text);
      }
    }

    private string SetFileName(int aCurentTest)
    {
      int cLength = mTotalTests.ToString().Length;
      int iLength = aCurentTest.ToString().Length;
      string name = subDirName + "_";
      for (int j = 0; j < cLength - iLength; j++)
      {
        name += "0";
      }
      return name + aCurentTest;
    }


    public override string GetFinalLog()
    {
      string str = mOutput + string.Format("\r\n{0} файлов записано.\r\n{1} текстов отклонено компилятором, из них {2} идущих подряд.", SavedCount, TotalBadResults, BadResults);
      File.WriteAllText(Path.Combine(TargetFolder, "output.txt"), str, Encoding.Default);
      return str;
    }
  }
}