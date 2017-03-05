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

namespace LiveGram
{
  /// <summary>
  /// Логика взаимодействия для Rule.xaml
  /// </summary>
  public partial class RuleControl : UserControl
  {
    Rule _rule;

    public RuleControl(Rule aRule)
      : this()
    {
      _rule = aRule;
      leftName.Content = _rule.Name;

      ControlFactory.CurrentRule = _rule.Name;
      UIElement control = ControlFactory.CreateControl(_rule.RightSide, new List<string>());
      rightPanel.Children.Add(control);
    }

    public RuleControl()
    {
      InitializeComponent();
    }
  }
}
