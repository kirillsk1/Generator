 
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using bnfGenerator.Properties;
using GrammarCompiler;
using GrammarCompiler.PhraseHierarchy;
using GrammarCompiler.ResultSaver;
using GrammarCompiler.Transforms;
using PrePostProcessing;
using WeifenLuo.WinFormsUI.Docking;

namespace bnfGenerator
{
  public partial class MainForm : Form
  {
    private const string VALID_PROCESSING_PATH = @"\ProcessingValidating\validating\";

    #region Все что касается DockPanel

    private readonly GrammarDoc mGrammarDoc = new GrammarDoc();
    private readonly GenerationOptionsForm mGenerationOptionsForm = new GenerationOptionsForm();
    private readonly OutputDoc mOutputDoc = new OutputDoc();
    private readonly ErrorList mErrorList = null;
    private readonly DerivationTree mDerivationTree = new DerivationTree();
    private readonly TaskDoc mTaskDoc = new TaskDoc();
    private readonly GraphWindow mGraphWindow = new GraphWindow();
    private readonly LevelListDoc mLevelListDoc = new LevelListDoc();

    private const string DOCK_CONFIG_FILE = @"\Configuration\DockManager.config";

    private bool LoadDockFromCfg()
    {
      //load DockManager config
      string configFile = Program.WorkingDir + DOCK_CONFIG_FILE;
      if (File.Exists(configFile))
      {
        try
        {
          dockPanel1.LoadFromXml(configFile, GetContentFromPersistString);
          return true;
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
          try
          {
            File.Delete(configFile);
          }
          catch
          {
          }
        }
      }
      return false;
    }

    private IDockContent GetContentFromPersistString(string persistString)
    {
      if (persistString == typeof (GrammarDoc).ToString())
      {
        return mGrammarDoc;
      }
      else if (persistString == typeof (OutputDoc).ToString())
      {
        return mOutputDoc;
      }
      else if (persistString == typeof (ErrorList).ToString())
      {
        return mErrorList;
      }
      else if (persistString == typeof (DerivationTree).ToString())
      {
        return mDerivationTree;
      }
      else if (persistString == typeof (GenerationOptionsForm).ToString())
      {
        return mGenerationOptionsForm;
      }
      else if (persistString == typeof (TaskDoc).ToString())
      {
        return mTaskDoc;
      }
      else if (persistString == typeof (GraphWindow).ToString())
      {
        return mGraphWindow;
      }
      else if (persistString == typeof(LevelListDoc).ToString())
      {
        return mLevelListDoc;
      }
      return null; //document;
    }

    #endregion

    private Generator mGenerator;

    public MainForm()
    {
      InitializeComponent();
      backgroundWorker1.DoWork += backgroundWorker1_DoWork;
      backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
      backgroundWorker1.WorkerSupportsCancellation = true;
      Grammar.GraphBuilder = mGraphWindow.GraphBuilder;

      //Init DockWindows
      mErrorList = new ErrorList(mGrammarDoc);

      OutputDoc mDebugOutputDoc = null;
      if (Lexer.DebugMode)
      {
        mDebugOutputDoc = new OutputDoc();
        mDebugOutputDoc.TabText = "Lexer Debug";
        mGrammarDoc.mLexerDebugOutput = mDebugOutputDoc;
      }

      if (!LoadDockFromCfg())
      {
        mErrorList.Show(dockPanel1, DockState.DockBottomAutoHide);
        mDerivationTree.Show(dockPanel1, DockState.DockLeftAutoHide);
        mGraphWindow.Show(dockPanel1, DockState.DockLeftAutoHide);
        mLevelListDoc.Show(dockPanel1, DockState.DockLeftAutoHide);

        if (Lexer.DebugMode)
        {
          mDebugOutputDoc.Show(dockPanel1, DockState.Document);
        }
        mOutputDoc.Show(dockPanel1, DockState.Document);

        mTaskDoc.Show(dockPanel1, DockState.DockLeft);
        mGenerationOptionsForm.Show(dockPanel1, DockState.DockLeft);
        mGrammarDoc.Show(dockPanel1, DockState.DockLeft);               
      }

      mTaskDoc.Init();
      mGrammarDoc.Init();
      mGrammarDoc.mDerivationTree = mDerivationTree;

      mGenerationMode = eGenerationMode.IterativeTopDown;
      iterativeTopDownToolStripMenuItem.Checked = true;
      recursiveTopDownToolStripMenuItem.ToolTipText = "Рекурсивный обход сверху вниз, слева направо";
      iterativeTopDownToolStripMenuItem.ToolTipText = @"Обход такой же, как и RecursiveTopDown, только выполняется итеративно, чтобы избежать возможности StackOverflow
Это основной рекомендуемый режим, поскольку он необходим для реализации семантических связей (обращение к уже выведенной части программы).";
      iterativeLeftRightToolStripMenuItem.ToolTipText = @"Вывод по слоям (слева направо сверху вниз).
Для демонстрации";

      mLevelListDoc.OutputDoc = mOutputDoc;
    }

    private eGenerationMode mGenerationMode;

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      //try
      //{
        //Generate
        mGenerator.ConfigFilePath = mTaskDoc.CurrentFilePath;
        //!!!!!DEBUG This (updating graph) is EXTREMLY SLOW !!!((((
        //Grammar.SpecGraphBuilder = new SpecificGraphBuilder(mGraphWindow.GraphBuilder);

        if (mGenerationOptionsForm.cbSaveToFiles.Checked)
        {
          mGenerator.ResultSaver = new FileResultSaver(mGenerationOptionsForm.txtPathSaveToFile.Text, mGenerationOptionsForm.txtLabelDir.Text, mGenerator.Options.TestsAmount);
        }
        mGenerator.ResultSaver.ProcessText += ResultSaver_ProcessText;
        
        string s = mGenerator.GenerateAll();          
        e.Result = s;
    //  }
    //  catch (Exception ex)
    //  {
    //    MessageBox.Show("Unhandled excpetion in backgroundWorker1_DoWork: " + ex);
     // }
    }

    void ResultSaver_ProcessText(object sender, ProcessTextEventArgs e)
    {
      int n = mGenerator.ResultSaver.SavedCount + 1;
      BeginInvoke((ThreadStart) delegate
                                  {
                                    lblStatTest.Text = string.Format("Test {0} / {1}", n, mGenerator.Options.TestsAmount);
                                  });
      // Форматирование и построцессинг
      string s = e.Text;
      if (mGenerationOptionsForm.cbPPprocessingEnabled.Checked)
      {
        string configDir = Program.WorkingDir + VALID_PROCESSING_PATH;
        string[] myArgs = new string[4] { "-c-10", "-l-C", "-p-" + configDir + "config.xml", "-s-ForTests" };
        ArgsParser aParser = new ArgsParser(myArgs);
        ConfigParser parser = new ConfigParser(aParser.configTest, configDir + @"\members.xml");
        replaceHolding sFormat =
          new replaceHolding(s, parser.inds, parser.arrayLength(), parser.IndexCount());
        s = sFormat.PrintText();
      }
      else
      {
        s = Formatter.Format(s);
      }
      e.Text = s;
      // Проверка компилятором
      if (mGenerationOptionsForm.cbMinGWEnabled.Checked)
      {
        MinGWProcessing checkMinGW = new MinGWProcessing();
        bool testPassed = checkMinGW.MinGW(s);
        e.CancelSave = !testPassed;        
        if (e.CancelSave)
        {
          e.CancelReason = checkMinGW.WarningString;
        } else
        {
          if (s.Length < 1024)
          {
            e.CancelSave = true;
            e.CancelReason = "Result is too short";
          }
        }
      }
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string s = (string) e.Result;
      mOutputDoc.SetText(s);
      if (mGenerator.DerivTree != null)
      {
        mDerivationTree.SetTree(mGenerator.DerivTree);
      }
      toolStripMenuItem1.Enabled = true;
      toolStripMenuStop.Enabled = false;
    }

    private void grammarOutputToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // OutPutGrammar
      toolStripMenuItem1.Enabled = false;
      toolStripMenuStop.Enabled = true;

      if (useToQuantifiersTransformToolStripMenuItem.Checked)
      {
        new ToQuantifiers().Transform(mGrammarDoc.mGrammar);
        mGrammarDoc.RefreshGrammarTree();
      }
      if (mGenerator != null)
      {
        mGenerator.Progress -= mGenerator_Progress;      
      }
      mGenerator = new Generator(mGrammarDoc.mGrammar);
      mGenerator.Options = mGenerationOptionsForm.GetOptions();
      mGenerator.Mode = mGenerationMode;
      mGenerator.Progress += mGenerator_Progress;
      backgroundWorker1.RunWorkerAsync();
    }

    void mGenerator_Progress(object sender, GenerateProgressEventArgs e)
    {
      mOutputDoc.BeginInvoke(new EventHandler<GenerateProgressEventArgs>(updateProgress), sender, e);      
    }

    void updateProgress(object sender, GenerateProgressEventArgs e)
    {
      //string s = Formatter.Format(e.RootText);
      //mOutputDoc.SetText(s);
      lblStatCount.Text = string.Format("Level {0} Max {1} CurInRoot {2} / {3} CurInCur {4}", e.CurrentListLevel, e.MaxLevel, e.CurrentInRoot, e.TotalInRoot, e.CurrentInCurrent);
      if (mGenerator.StopGenerate)
      {
        lblStatCount.Text += "Stopping";
      }
      lblStatCur.Text = e.CurrentListText;
      
      if (e.PausedAtList != null)
      {
        //Update debug window
        mLevelListDoc.Clear();
        ListDerivation p = e.PausedAtList;
        while (p != null)
        {
          string text = string.Format("{0} {1} {2}", p.Level, p.ExpandingSymbol, p.mCurrentItemIndex);
          mLevelListDoc.Add(text, p);
          p = p.ParentDerivation as ListDerivation;
        }
      }
    }

    private void lexerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        string s = mGrammarDoc.Text; // @"""main (123)"", "" print """" hello"" a | stat";
        Lexer l = new Lexer(s);
        Lexema lex;
        do
        {
          lex = l.GetNext();
        } while (lex != null);
        mOutputDoc.SetText(l.DebugText);
        //string s1 = @"""main (123)";
        //Lexer l1 = new Lexer(s1);
        //mOutputDoc.SetText(l1.DebugText);
      }
      catch (GrammarSyntaxException ex)
      {
        mErrorList.AddError(ex);
      }
    }

    private void Stop_Click(object sender, EventArgs e)
    {
      if (toolStripMenuStop.Enabled)
      {
        toolStripMenuStop.Enabled = false;
        backgroundWorker1.CancelAsync();
      }
    }

    private void savePositionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      dockPanel1.SaveAsXml(Program.WorkingDir + DOCK_CONFIG_FILE);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.LastGrammar = Path.GetFileName(mGrammarDoc.CurrentFilePath);
      Settings.Default.Save();
    }

    private void grammarIndexingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // Grammar Indexing
      XmlDocument grammarIndexingFile = new XmlDocument();
      XmlNode root = grammarIndexingFile.CreateElement("Root");
      grammarIndexingFile.AppendChild(root);

      foreach (Rule tmpRule in mGrammarDoc.mGrammar.Rules.Values)
      {
        XmlNode tmpRuleName = grammarIndexingFile.CreateElement(tmpRule.Name);
        AlternativeSet lAlt = tmpRule.RightSide as AlternativeSet;
        int cnt = 0;
        if (lAlt != null)
        {
          cnt = lAlt.Count;
        }
        XmlAttribute rrAtr = grammarIndexingFile.CreateAttribute("rr");
        //rrAtr.Value = "10";
        string attrValue = "10";
        tmpRuleName.Attributes.Append(rrAtr);
        for (int i = 1; i < cnt; i++)
        {
          attrValue += " 1";
        }
        rrAtr.Value = attrValue;
        tmpRuleName.Attributes.Append(rrAtr);
        root.AppendChild(tmpRuleName);
      }
      grammarIndexingFile.Save(@"C:\grammarIndexing.xml");
    }

    private void tbGenOneProg_Click(object sender, EventArgs e)
    {
      ;
    }

    private void tbGenStep_Click(object sender, EventArgs e)
    {
      ;
    }

    private void useToQuantifiersTransformToolStripMenuItem_Click(object sender, EventArgs e)
    {
      useToQuantifiersTransformToolStripMenuItem.Checked ^= true;
      if (useToQuantifiersTransformToolStripMenuItem.Checked)
      {
        TransformStatistics stat = new ToQuantifiers().Transform(mGrammarDoc.mGrammar);
        MessageBox.Show(stat.ToString());
        mGrammarDoc.RefreshGrammarTree();
      }
      Settings.Default.Save();
    }

    #region Gen Mode select
    private void recursiveTopDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
      recursiveTopDownToolStripMenuItem.Checked = true;
      iterativeTopDownToolStripMenuItem.Checked = false;
      iterativeLeftRightToolStripMenuItem.Checked = false;
      mGenerationMode = eGenerationMode.RecursiveTopDown;
    }

    private void iterativeTopDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
      recursiveTopDownToolStripMenuItem.Checked = false;
      iterativeTopDownToolStripMenuItem.Checked = true;
      iterativeLeftRightToolStripMenuItem.Checked = false;
      mGenerationMode = eGenerationMode.IterativeTopDown;
    }

    private void iterativeLeftRightToolStripMenuItem_Click(object sender, EventArgs e)
    {
      recursiveTopDownToolStripMenuItem.Checked = false;
      iterativeTopDownToolStripMenuItem.Checked = false;
      iterativeLeftRightToolStripMenuItem.Checked = true;
      mGenerationMode = eGenerationMode.IterativeLeftRight;
    }
    #endregion

    private void btnPause_Click(object sender, EventArgs e)
    {
      mGenerator.PauseGenerate ^= true;
      btnPause.Checked = mGenerator.PauseGenerate;
    }

    private void m_Cl(object sender, EventArgs e)
    {
      Grammar gr = Grammar.FromTextFile("c:/grams/gramset/gram800.txt");
      Generator generator = new Generator(gr);
      generator.Options.LevelRestriction = 60;
      generator.Options.AlternativeAlg = AlternativeSelectAlg.NormalDistr;
      generator.Options.TestsAmount = 1;
      generator.Options.AllEnumInOne = false;
      string s = generator.GenerateAll();


      if (mGenerationOptionsForm.cbPPprocessingEnabled.Checked)
      {
        string configDir = Program.WorkingDir + VALID_PROCESSING_PATH;
        string[] myArgs = new string[4] { "-c-10", "-l-C", "-p-" + configDir + "config.xml", "-s-ForTests" };
        ArgsParser aParser = new ArgsParser(myArgs);
        ConfigParser parser = new ConfigParser(aParser.configTest, configDir + @"\members.xml");
        replaceHolding sFormat =
          new replaceHolding(s, parser.inds, parser.arrayLength(), parser.IndexCount());
        s = sFormat.PrintText();
      }
      else
      {
        s = Formatter.Format(s);
      }
     // e.Text = s;
      // Проверка компилятором

      if (mGenerationOptionsForm.cbMinGWEnabled.Checked)
      {
        MinGWProcessing checkMinGW = new MinGWProcessing();
        bool testPassed = checkMinGW.MinGW(s);
        //  e.CancelSave = !testPassed;        
        if (!testPassed)
        {
          //  e.CancelReason = checkMinGW.WarningString;
        }
        else
        {
          File.WriteAllText("c:/gram_pv/sc1deps_grams", s);
        }

      }
    }

    private void generateBySetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      //generator.
      DirectoryInfo dir = new DirectoryInfo("c:/gram_pv/pv_deps_grams");
      string pathToGrammar = "";
      string fileToSaveTest = "";
      bool testPassed = false;
      bool TestGenerated = false;
      foreach (FileInfo f in dir.GetFiles())
      {
        pathToGrammar = f.FullName;
        fileToSaveTest = f.Name.Substring(4, f.Name.Length - 8);
        if (File.Exists("c:/gram_pv/pv_deps_tests/SF1D-test_" + fileToSaveTest + ".c")) continue;
        Grammar gr = Grammar.FromTextFile(pathToGrammar);
      Generator generator = new Generator(gr);
      generator.Options.LevelRestriction = 1000;
      generator.Options.AlternativeAlg = AlternativeSelectAlg.RndDistr;
      generator.Options.RREnable = true;
      generator.Options.TestsAmount = 1;
      generator.Options.AllEnumInOne = false;
      generator.ConfigFilePath = mTaskDoc.CurrentFilePath;
        TestGenerated = false;
      while (!TestGenerated)
      {
        string s = generator.GenerateAll();


        if (mGenerationOptionsForm.cbPPprocessingEnabled.Checked)
        {
          string configDir = Program.WorkingDir + VALID_PROCESSING_PATH;
          string[] myArgs = new string[4] {"-c-10", "-l-C", "-p-" + configDir + "config.xml", "-s-ForTests"};
          ArgsParser aParser = new ArgsParser(myArgs);
          ConfigParser parser = new ConfigParser(aParser.configTest, configDir + @"\members.xml");
          replaceHolding sFormat =
            new replaceHolding(s, parser.inds, parser.arrayLength(), parser.IndexCount());
          s = sFormat.PrintText();
        }
        else
        {
          s = Formatter.Format(s);
        }
        // e.Text = s;
        // Проверка компилятором
          MinGWProcessing checkMinGW = new MinGWProcessing();
          testPassed = checkMinGW.MinGW(s);
        
          if (testPassed)
          {
            File.WriteAllText("c:/gram_pv/pv_deps_tests/SF1D-test_" + fileToSaveTest + ".c", s);
            TestGenerated = true;
          }
          else
          {
            TestGenerated = false;
          }
      }
      }
    }
  }
}