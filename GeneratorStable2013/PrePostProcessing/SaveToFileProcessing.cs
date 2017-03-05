 
using System;
using System.IO;

namespace PrePostProcessing
{
  public class SaveToFileProcessing
  {
    private readonly DirectoryInfo mMainDir;
    private readonly DirectoryInfo subDir;
    public string mDefDirToSave = "c:\\testsdef\\";
    private readonly string subDirName = "";

    public SaveToFileProcessing(string SaveTo, string Label)
    {
      if (SaveTo != "def")
      {
        if (Directory.Exists(SaveTo))
        {
          mMainDir = new DirectoryInfo(SaveTo);
        }
        else
        {
          Directory.CreateDirectory(mDefDirToSave);
          mMainDir = new DirectoryInfo(mDefDirToSave);
        }
        subDirName = Label + "_" + DateTime.Now.ToString("hh-mm-ss-dd-MM-yyyy");
        if (mMainDir.Exists)
        {
          //DirectoryInfo subDir = mainDir.CreateSubdirectory(DateTime.Today.ToString("dd-mM-yyyy"));
          subDir = mMainDir.CreateSubdirectory(subDirName);
        }
      }
    }

    public void SaveFile(string Text, int TotalTests, int curentTest)
    {
      StreamWriter curFile =
        File.CreateText(subDir + "\\" + SetFileName(TotalTests, curentTest) + ".c");
      curFile.WriteLine(Text);
      curFile.Close();
    }


    private string SetFileName(int TotalTests, int curentTest)
    {
      int cLength = TotalTests.ToString().Length;
      int iLength = curentTest.ToString().Length;
      string name = subDirName + "_";
      for (int j = 0; j < cLength - iLength; j++)
      {
        name += "0";
      }
      return name += curentTest.ToString();
    }
  }
}