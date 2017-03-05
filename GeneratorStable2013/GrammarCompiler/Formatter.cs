 
using System;
using System.Text.RegularExpressions;

namespace GrammarCompiler
{
  public class Formatter
  {
    public static string Format(string s)
    {
      s = s.Replace("void main", "\r\nvoid main");
      s = s.Replace("{", "\r\n{\r\n");
      s = s.Replace("}", "\r\n}\r\n");
        
      string Spacing = "";
      string Res = "";
      Regex splitter = new Regex("\r\n");
      string[] lines = splitter.Split(s);
      foreach (string c in lines)
      {
        string d = c;
        switch (d)
        {
          case "{":
            Res += Spacing + "{\r\n";
            Spacing += "    ";
            break;
          case "}":
            if (Spacing.Length < 4) throw new FormatException("непарная закрывающая скобка }");
            Spacing = Spacing.Remove(0, 4);
            Res += Spacing + "}\r\n";
            break;
          default:
            Res += Spacing + d + "\r\n";
            break;
        }
      }
      return Res;
    }
  }
}