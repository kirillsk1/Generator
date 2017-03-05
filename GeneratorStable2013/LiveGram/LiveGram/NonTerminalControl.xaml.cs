using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GrammarCompiler;
using System.Threading;
using System.Windows.Threading;
using GrammarCompiler.PhraseHierarchy;

namespace LiveGram
{
  /// <summary>
  /// Логика взаимодействия для NonTerminal.xaml
  /// </summary>
  public partial class NonTerminalControl : UserControl
  {
    NonTerminal _symbol;
    List<string> _outerSymbols;

    static Dispatcher mDispatcher;
    static bool mPause = false;
    public static bool Pause
    {
      set
      {
        mPause = value;
        if (!value)
        {
          ExecuteDelegates();
        }
      }
    }

    private static void ExecuteDelegates()
    {
      while (nextDelegateList.Count > 0)
      {
        //continue
        ThreadStart nextDelegate = nextDelegateList[0];
        nextDelegateList.RemoveAt(0);
        mDispatcher.BeginInvoke(nextDelegate, DispatcherPriority.SystemIdle);
      }
    }

    public static void DoStep()
    {
      ExecuteDelegates();
    }

    public static List<ThreadStart> nextDelegateList = new List<ThreadStart>();
    public static int maxDeep = 0;
    public static List<string> diffCycles = new List<string>();

    public NonTerminalControl(NonTerminal aSymbol, List<string> aOuterSymbols)
      : this()
    {
      mDispatcher = this.Dispatcher;
      _symbol = aSymbol;
      _outerSymbols = new List<string>(aOuterSymbols);
      if (_outerSymbols.Count > maxDeep)
      {
        maxDeep = _outerSymbols.Count;
        ControlFactory.MainWindow.MaxDeep = maxDeep;
      }
      
      bool isUsed = ControlFactory.AllUsedSymbols.Contains(aSymbol.Text);
      ControlFactory.AllUsedSymbols.Add(aSymbol.Text);

      ThreadStart nextDelegate = (ThreadStart)delegate()
      {
        MainWindow.AllTrees.Add(treeView);

        TreeViewItem tvi = new TreeViewItem();
        tvi.IsExpanded = MainWindow.DefaultExpand;
        treeView.Items.Add(tvi);


        Button b = new Button();
        b.Content = _symbol.Text;
        tvi.Header = b;
        if ((_symbol.CycicKind & CycicKind.CyclicPropagated) >0 )
        {
          b.Foreground = Brushes.Magenta;
        }
        if (_symbol.IsCyclic)
        {
          b.Foreground = Brushes.Blue;
        }

        if (!_outerSymbols.Contains(_symbol.Text) && _symbol.Text != ControlFactory.CurrentRule)
        {
          if (isUsed)
          {
            // as cycle

            b.Template = (ControlTemplate)Application.Current.Resources["secondTemplate"];
            b.Content = _symbol.Text + " *";
            b.Click += new RoutedEventHandler(b_Click);
          }
          else
          {
            _outerSymbols.Add(_symbol.Text);

            //No cycle
            UIElement ui;
            Dictionary<string, Rule> rules = _symbol.Grammar.Rules;
            if (rules.ContainsKey(_symbol.Text))
            {
              Rule r = rules[_symbol.Text];
              ui = ControlFactory.CreateControl(r.RightSide, _outerSymbols);
            }
            else
            {
              Label l = new Label();
              l.Content = "rule not found!";
              ui = l;
            }
            TreeViewItem new_tvi = new TreeViewItem();
            new_tvi.Header = ui;
            tvi.Items.Add(new_tvi);
          }
        }
        else
        {
          //cycle
          ControlFactory.MainWindow.AllCycles++;
          if (!diffCycles.Contains(_symbol.Text))
          {
            diffCycles.Add(_symbol.Text);
            ControlFactory.MainWindow.AllDiffCycles = diffCycles.Count;
          }

          if (_symbol.Text != ControlFactory.CurrentRule)
            b.Template = (ControlTemplate)Application.Current.Resources["cycleTemplate"];
          else
            b.Template = (ControlTemplate)Application.Current.Resources["selfCycleTemplate"];
          b.Content = _symbol.Text + " ^";
          b.Click += new RoutedEventHandler(b_Click);
        }
      };
      nextDelegateList.Add(nextDelegate);

      if (!mPause)
      {
        ExecuteDelegates();
      }
    }

    void b_Click(object sender, RoutedEventArgs e)
    {
      Button b = sender as Button;
      string text = (string) b.Content;

      ContentControl p;
      do
      {
        p = b.Parent as ContentControl;
        //if (p is TreeViewItem)
        //{          
        //}
      }
      while (p != null && (!(p is TreeView) && (string)p.Content != text));
      if (p != null)
      {
        p.Background = Brushes.Green;
        p.Focus();
      }
    }

    public NonTerminalControl()
    {
      InitializeComponent();
    }
  }
}
