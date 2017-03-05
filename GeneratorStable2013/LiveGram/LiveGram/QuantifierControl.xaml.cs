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
using GrammarCompiler.PhraseHierarchy;

namespace LiveGram
{
  /// <summary>
  /// Логика взаимодействия для QuantifierControl.xaml
  /// </summary>
  public partial class QuantifierControl : UserControl
  {
    QuantifiedPhrase _quantifiedPhrase;

    public QuantifierControl(QuantifiedPhrase aQuantifiedPhrase, List<string> aOuterSymbols)
      : this()
    {
      _quantifiedPhrase = aQuantifiedPhrase;

      UIElement control = ControlFactory.CreateControl(_quantifiedPhrase.Phrase, aOuterSymbols);
      quantPanel.Children.Add(control);

      quantLabel.Content = _quantifiedPhrase.QuantSign;
      //if (_quantifiedPhrase.Min == _quantifiedPhrase.Max)
      //{
      //  quantLabel.Content = "{" + _quantifiedPhrase.Max + "}";
      //}
      ////else if (_quantifiedPhrase.Max == Int32.MaxValue)
      ////{
      ////  quantLabel.Content = (_quantifiedPhrase.Min == 1) ? "+" : "*";
      ////}
      //else
      //{
      //  string max = (_quantifiedPhrase.Max == Int32.MaxValue) ? "*" : _quantifiedPhrase.Max.ToString();
      //  quantLabel.Content = "{" + _quantifiedPhrase.Min + ".." + max + "}";
      //}
    }

    public QuantifierControl()
    {
      InitializeComponent();
    }
  }
}
