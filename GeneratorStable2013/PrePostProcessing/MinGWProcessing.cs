 
using System.Diagnostics;
using System.IO;

namespace PrePostProcessing
{
  public class MinGWProcessing
  {
    private readonly string pathToMinGW;

    public string WarningString;

    public MinGWProcessing()
    {
      pathToMinGW = Program.WorkingDir + @"/MinGW/bin/gcc.exe";
    }

    public bool MinGW(string aTestFileString)
    {
      bool passedTest = false;
      string tmpFileName = /*Path.GetTempPath() + "\\*/ "tmpName.c";
      using (StreamWriter tmpFile = File.CreateText(tmpFileName))
      {
        tmpFile.WriteLine(aTestFileString);
      }
      FileInfo fSize = new FileInfo(tmpFileName);
      int Size = (int) fSize.Length;
      int TimeToExit = 0;
      ProcessStartInfo proc = new ProcessStartInfo(pathToMinGW, tmpFileName);
      proc.RedirectStandardError = true;
      proc.UseShellExecute = false;
      Process p = Process.Start(proc);
      //p.WaitForExit(10000);
      while (!p.HasExited && TimeToExit < 2000000)
      {
        TimeToExit++;
        //Console.WriteLine(TimeToExit);
      }
      if (TimeToExit > 2000000)
      {
        passedTest = false;
      }
      else
      {


        WarningString = p.StandardError.ReadToEnd();
        //p.Dispose();
        if (WarningString.Contains("error"))
        {
          passedTest = false;
        }
        else
        {
          passedTest = true;
          if (Size > 50*1024)
          {
            //Console.WriteLine("too long");
          }
        }
      }
      p.Dispose();
      File.Delete(tmpFileName);
      return passedTest;
    }
  }
}