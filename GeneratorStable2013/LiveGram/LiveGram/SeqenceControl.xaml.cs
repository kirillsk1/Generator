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
  /// Логика взаимодействия для SeqenceControl.xaml
  /// </summary>
  public partial class SeqenceControl : UserControl
  {
    Seqence _seqence;
    static Thickness t = new Thickness(2);
    public SeqenceControl(Seqence aSeqence, List<string> aOuterSymbols)
      : this()
    {
      _seqence = aSeqence;

      foreach (IPhrase phrase in _seqence.Phrases)
      {
        UIElement control = ControlFactory.CreateControl(phrase, aOuterSymbols);
        Border brd = new Border();
        brd.Child = control;
        brd.BorderThickness = t;
        brd.BorderBrush = Brushes.Magenta;
        stackPanel1.Children.Add(brd);
      }
    }
    public SeqenceControl()
    {
      InitializeComponent();
    }
  }
}
