 
using System;
using System.IO;
using System.Windows.Forms;
using bnfGenerator.Properties;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;

namespace bnfGenerator
{
  public partial class SciEditDoc : ToolWindow
  {
    private string mWorkingDir;
    private string mCurrentFile = "";
    public string WorkSubDir;
    public string FileExt = "*.txt";
    public string SciLang = "xml";
    public new string Text;
    public FileSystemWatcher fiWatch;

    public string CurrentFilePath
    {
      get { return mCurrentFile; }
      set
      {
        mCurrentFile = value;
        OpenDoc();
      }
    }

    private static SciTEProperties properties;
    private static ScintillaConfig mScintillaConfig = null;
    private bool StopOpenDocFlag = false;

    public static ScintillaConfig ScintillaConfig
    {
      get
      {
        if (mScintillaConfig == null)
        {
          FileInfo globalConfigFile = new FileInfo(Program.WorkingDir + @"\Configuration\global.properties");
          if (globalConfigFile.Exists)
          {
            properties = new SciTEProperties();
            properties.Load(globalConfigFile);
            mScintillaConfig = new ScintillaConfig(properties);
          }
        }
        return mScintillaConfig;
      }
    }

    public void Init()
    {
      btnSave.Enabled = false;
      mWorkingDir = Program.WorkingDir;
      //Load sci config
      scintillaControl1.Configuration = ScintillaConfig;
      mWorkingDir = Path.Combine(mWorkingDir, WorkSubDir);

      fiWatch = new FileSystemWatcher(mWorkingDir);
      fiWatch.Created += fiWatch_Handler;
      fiWatch.Deleted += fiWatch_Handler;
      fiWatch.Changed += fiWatch_Changed;
      fiWatch.EnableRaisingEvents = true;

      if (Directory.Exists(mWorkingDir))
      {
        FillDirCombo();
      }
      TrySetSelectedFile(Settings.Default.LastGrammar);
    }

    private delegate void SimpleDelegate();

    private delegate void StrDelegate(string aName);

    private void fiWatch_Handler(object sender, FileSystemEventArgs e)
    {
      Invoke(new SimpleDelegate(UpdateDirCombo));
    }

    private void fiWatch_Changed(object sender, FileSystemEventArgs e)
    {
      Invoke(new StrDelegate(ReloadDoc), e.FullPath);
    }

    private void ReloadDoc(string aFullPath)
    {
      if (aFullPath == CurrentFilePath)
      {
        if (DialogResult.Yes ==
            MessageBox.Show(string.Format("Файл {0} был изменен кем-то из вне. Вы хотите перечитать его?", aFullPath),
                            "Перечитать", MessageBoxButtons.YesNo))
        {
          OpenDoc();
        }
      }
    }

    private void UpdateDirCombo()
    {
      StopOpenDocFlag = true; //Запрещаем загрузку документа в обратотчике comboBox1_SelectedIndexChanged
      FillDirCombo();
      TrySetSelectedFile(Path.GetFileName(CurrentFilePath));
      StopOpenDocFlag = true;
    }

    private void TrySetSelectedFile(string aName)
    {
      int i = comboBox1.Items.IndexOf(aName);
      if (i >= 0)
      {
        comboBox1.SelectedIndex = i;
      }
    }

    private void FillDirCombo()
    {
      comboBox1.Items.Clear();
      comboBox1.Sorted = true;
      DirectoryInfo mDirInfo = new DirectoryInfo(mWorkingDir);
      foreach (FileInfo fi in mDirInfo.GetFiles(FileExt))
      {
        comboBox1.Items.Add(fi.Name);
      }
      if (comboBox1.Items.Count == 0)
      {
        comboBox1.Items.Add(string.Format("There are no {0} files here", FileExt));
        MessageBox.Show(string.Format("There are no {0} files here", FileExt));
        return;
      }
      comboBox1.SelectedIndex = 0;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (StopOpenDocFlag) return;
      mCurrentFile = Path.Combine(mWorkingDir, comboBox1.Text);
      OpenDoc();
    }

    private void OpenDoc()
    {
      if (File.Exists(mCurrentFile))
      {
        OpenFile();
      }
      ReParse();
    }

    private void OpenFile()
    {
      btnSave.Enabled = true;
      Text = File.ReadAllText(mCurrentFile);
      scintillaControl1.Text = Text;
      scintillaControl1.SelectionAlpha = 100;
      scintillaControl1.ConfigurationLanguage = SciLang;
      //richTextBox1.Text = File.ReadAllText(mCurrentFile);
    }

    protected virtual void ReParse()
    {
      Text = scintillaControl1.Text;
    }

    /*bug repeated
     * public string[] Lines
    {
      get
      {
        string[] lRes = new string[scintillaControl1.LineCount];
        for (int i = 0; i < scintillaControl1.LineCount; i++)
        {
          lRes[i] = scintillaControl1.Line[i];
        }
        return lRes;
      }
    }*/

    internal void MarkError(int aLine, int aCol)
    {
      scintillaControl1.MarkerAdd(aLine, 1);
      scintillaControl1.SelectionStart = scintillaControl1.PositionFromLine(aLine) + aCol;
      scintillaControl1.SelectionEnd = scintillaControl1.SelectionStart + 1;
      /*string text = richTextBox1.Lines[aLine];
      int st = richTextBox1.Find(text);
      richTextBox1.SelectionStart = st + aCol;
      richTextBox1.SelectionLength = 3; //text.Length;
      //richTextBox1.SelectionBackColor = Color.Red;
      richTextBox1.Focus();*/
    }


    private void btnSave_Click(object sender, EventArgs e)
    {
      //File.WriteAllText(mCurrentFile, richTextBox1.Text);
      File.WriteAllText(mCurrentFile, scintillaControl1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog lDlg = new FolderBrowserDialog();
      lDlg.SelectedPath = mWorkingDir;
      if (lDlg.ShowDialog() == DialogResult.OK)
      {
        mWorkingDir = lDlg.SelectedPath;
        fiWatch.Path = mWorkingDir;
        FillDirCombo();
      }
    }

    public SciEditDoc()
    {
      InitializeComponent();
    }

    private void btnReParse_Click_1(object sender, EventArgs e)
    {
      ReParse();
    }
  }
}