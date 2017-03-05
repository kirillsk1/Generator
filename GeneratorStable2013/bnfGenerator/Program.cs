 
using System;
using System.IO;
using System.Windows.Forms;

namespace bnfGenerator
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    public static string WorkingDir
    {
      get
      {
        string mWorkingDir = Application.StartupPath;
        if (mWorkingDir.EndsWith(@"\bin\Debug"))
        {
          mWorkingDir = mWorkingDir.Replace(@"\bin\Debug", "");
          mWorkingDir = Path.GetDirectoryName(mWorkingDir);
        }
        return mWorkingDir;
      }
    }
  }
}