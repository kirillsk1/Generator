using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using GrammarCompiler;
using Microsoft.Win32;
using GrammarCompiler.Transforms;
using System.Diagnostics;

namespace LiveGram
{
  /// <summary>
  /// Логика взаимодействия для Window1.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    string grammar_name;
    Grammar g;
    public MainWindow()
    {
      InitializeComponent();
    }

    public static bool DefaultExpand = true;
    //public static DependencyProperty DefaultExpandProperty = DependencyProperty.Register(
    //    "DefaultExpand", typeof(bool), typeof(MainWindow));
    //public bool DefaultExpand
    //{
    //  get { return (bool)GetValue(DefaultExpandProperty); }
    //  set { SetValue(DefaultExpandProperty, value); }
    //}

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      GetGramName();
    }

    private void GetGramName()
    {
      grammar_name = Properties.Settings.Default.GrammarName;
      if (File.Exists(grammar_name))
      {
        Title = "Live Gram - " + grammar_name;
      }
      else grammar_name = null;
    }
    private void ParseGrammar()
    {
      if (string.IsNullOrEmpty(grammar_name)) return;

        btnStep.IsEnabled = true;
        ControlFactory.MainWindow = this;
        //reset stats
        AllNonTerminals = 0;
        AllSequenses = 0;
        AllAlternativeSets = 0;
        NonTerminalControl.maxDeep = 0;
        MaxDeep = 0;
        AllCycles = 0;
        NonTerminalControl.diffCycles.Clear();
        AllDiffCycles = 0;
        ControlFactory.AllUsedSymbols.Clear();

        Dispatcher.BeginInvoke( (Action) delegate()
              {
                try
                {
                InitGram();

                if (cbAllRules.IsChecked.Value)
                {
                  ListView rulesList = new ListView();
                  mainPanel.Children.Add(rulesList);
                  foreach (Rule r in g.Rules.Values)
                  {
                    RuleControl rc = new RuleControl(r);
                    rulesList.Items.Add(rc);
                  }
                }
                else
                {
                  Rule r = g.Rules[g.MainSymbol.Text];
                  RuleControl rc = new RuleControl(r);
                  mainPanel.Children.Add(rc);
                }
                }
                catch (Exception ex)
                {
                  MessageBox.Show("Ошибка при разборе грамматики: " + ex);
                }
              });
    }
    private void InitGram()
    {
      g = new GrammarCompiler.Grammar(File.OpenText(grammar_name));
      if (cbToQuantifiers.IsChecked.Value)
      {
        TransformStatistics stat = new ToQuantifiers().Transform(g);
        cbToQuantifiers.ToolTip = stat.ToString();
      }
    }
    private void OpenGrammar(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.FileName = Properties.Settings.Default.GrammarName;
      bool? ok = dialog.ShowDialog();
      if (ok.HasValue && ok.Value)
      {
        Properties.Settings.Default.GrammarName = dialog.FileName;
        Properties.Settings.Default.Save();
        OpenCurrentGrammar();
      }
    }

    //private WorkerThread mWorkerThread = null;
    private void OpenCurrentGrammar()
    {
      //if (mWorkerThread == null)
      //{
      //  mWorkerThread = new WorkerThread();
      //}
      AllTrees.Clear();
      mainPanel.Children.Clear();
      DefaultExpand = cbDefaultExpand.IsChecked.Value;

      GetGramName();
      ParseGrammar();
    }

    #region Expand / Collapse all
    public static List<TreeView> AllTrees = new List<TreeView>();

    private void handleExpandAll(object sender, RoutedEventArgs e)
    {
      Expand(true);
    }

    private void handlCollapseAll(object sender, RoutedEventArgs e)
    {
      Expand(false);
    }

    private void Expand(bool p)
    {
      foreach (TreeView tree in AllTrees)
        foreach (object item in tree.Items)
        {
          TreeViewItem treeItem = tree.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
          if (treeItem != null)
          {
            ExpandAll(treeItem, p);
            treeItem.IsExpanded = p;
          }
        }
    }

    void ExpandAll(ItemsControl items, bool expand)
    {
      foreach (object obj in items.Items)
      {
        ItemsControl childControl = items.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
        if (childControl != null)
        {
          ExpandAll(childControl, expand);
        }
        TreeViewItem item = childControl as TreeViewItem;
        if (item != null)
          item.IsExpanded = true;
      }
    }
    #endregion
  
    private void handlRefresh(object sender, RoutedEventArgs e)
    {
      OpenCurrentGrammar();
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
    {
      NonTerminalControl.Pause = cbPause.IsChecked.Value; 
    }

    private void btnStep_Click(object sender, RoutedEventArgs e)
    {
      if (NonTerminalControl.nextDelegateList.Count == 0)
      {
        btnStep.IsEnabled = false;
      }
      NonTerminalControl.DoStep();
    }

    #region Statistics
    int mAllNonTerminals;
    public int AllNonTerminals
    {
      get { return mAllNonTerminals; }
      set
      {
        mAllNonTerminals = value;
        lblNonTerminals.Content = value;
      }
    }

    int mAllSequenses;
    public int AllSequenses
    {
      get { return mAllSequenses; }
      set
      {
        mAllSequenses = value;
        lblSequenses.Content = value;
      }
    }

    int mAllAlternativeSets;
    public int AllAlternativeSets
    {
      get { return mAllAlternativeSets; }
      set
      {
        mAllAlternativeSets = value;
        lblAlternativeSets.Content = value;
      }
    }

    public int MaxDeep
    {
      set
      {
        lblMaxDeep.Content = value;
      }
    }

    int mAllCycles;
    public int AllCycles
    {
      get { return mAllCycles; }
      set
      {
        mAllCycles = value;
        lblCycles.Content = value;
      }
    }

    int mAllDiffCycles;
    public int AllDiffCycles
    {
      get { return mAllDiffCycles; }
      set
      {
        mAllDiffCycles = value;
        lblDiffCycles.Content = value;
        lblDiffCycles.ToolTip = string.Join("\r\n", NonTerminalControl.diffCycles.ToArray());
      }
    }
    #endregion

    private void btnGrammarToString_Click(object sender, RoutedEventArgs e)
    {
      if (g == null)
      {
        InitGram();
      }
      string path = @"grammar.txt";
      File.WriteAllText(path, g.ToString());
      Process.Start("notepad.exe", path);
    }

  }
}

