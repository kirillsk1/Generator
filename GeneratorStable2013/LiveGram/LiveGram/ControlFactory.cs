using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GrammarCompiler;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using GrammarCompiler.PhraseHierarchy;

namespace LiveGram
{
  public static class ControlFactory
  {
    public static string CurrentRule;
    public static MainWindow MainWindow;
    public static List<string> AllUsedSymbols = new List<string>();

    public static UIElement CreateControl(IPhrase element, List<string> aOuterSymbols)
    {
      if (element is Seqence)
      {
        MainWindow.AllSequenses++;
        return new SeqenceControl(element as Seqence, aOuterSymbols);
      }
      else if (element is AlternativeSet)
      {
        MainWindow.AllAlternativeSets++;
        return new AlternativeControl(element as AlternativeSet, aOuterSymbols);
      }
      else if (element is QuantifiedPhrase)
        return new QuantifierControl(element as QuantifiedPhrase, aOuterSymbols);
      else if (element is NonTerminal)
      {
        NonTerminal s = element as NonTerminal;
      
        MainWindow.AllNonTerminals++;
        return new NonTerminalControl(s, aOuterSymbols);
      
      }
      else if (element is Terminal)
      {
        Terminal s = element as Terminal;
      
        Label l = new Label();
        l.Content = s.Text;
        //Run r = new Run(s.Text);
        l.Foreground = Brushes.Red;
        return l;
      }
      

      Label unk = new Label();
      if (element is AccessSeq)
      {
        AccessSeq acc = element as AccessSeq;
        unk.Content = "#" + acc.ObjectName + "." + acc.mFieldName;
      }
      else if (element is TransCallPhrase)
      {
        TransCallPhrase trans = element as TransCallPhrase;
        unk.Content = "call " + trans.BindedMethod.Name;
      }
      else if (element is AccessArray)
      {
        AccessArray ar = element as AccessArray;
        unk.Content = ar.ObjectName + "[" + ar.IndexExpr.ToString() + "]";
      }
      else unk.Content = "unknown:" + element.ToString();
      return unk;
    }
  }
}
