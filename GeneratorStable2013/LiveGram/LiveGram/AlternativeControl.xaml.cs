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
  /// Логика взаимодействия для AlternativeControl.xaml
  /// </summary>
  public partial class AlternativeControl : UserControl
  {
    AlternativeSet _alternativeSet;

    public AlternativeControl(AlternativeSet aAlternativeSet, List<string> aOuterSymbols)
      : this()
    {
      _alternativeSet = aAlternativeSet;

      foreach (IPhrase phrase in _alternativeSet.Phrases)
      {
        Border brd = new Border();
        UIElement control = ControlFactory.CreateControl(phrase, aOuterSymbols);
        brd.Child = control;
        brd.BorderThickness = new Thickness(5);
        if (phrase.IsCyclic)
        {
          brd.BorderBrush = Brushes.Blue;
        }
        else
        {
          brd.BorderBrush = Brushes.YellowGreen;
        }
        stackPanel1.Children.Add(brd);
      }
    }

    public AlternativeControl()
    {
      InitializeComponent();
    }
  }
}
