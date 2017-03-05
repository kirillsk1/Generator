 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace PrePostProcessing
{
  internal static class ExtensionMethods
  {
    public static List<string> SplitToLines(this string s)
    {
      List<string> lines = new List<string>();
      using (StringReader sr = new StringReader(s))
      {
        string str = sr.ReadLine();
        while (str != null)
        {
          lines.Add(str); str = sr.ReadLine();
        }
      }
      return lines;
    }

    public static string JoinLines(this List<string> lines)
    {
      StringBuilder ResBld = new StringBuilder(200);
      foreach (string line in lines)
      {
        ResBld.AppendLine(line);
      }
      return ResBld.ToString();
    }
  }


  internal class Formatter
  {
    public static ArrayList rndLabels = new ArrayList();
    public static ArrayList rndLabelsForReplacement = new ArrayList();
   // public static string fRes = "";
   //private static Regex splitter = new Regex("\r\n", RegexOptions.Compiled);
    public static string Format(string s)
    {
      string Spacing = "";
      StringBuilder fRes = new StringBuilder(200);
      string[] lines = s.SplitToLines().ToArray();
      
      foreach (string c in lines)
      {
        string d = c;
        switch (d)
        {
          case "{":
            fRes.AppendLine(Spacing + "{");
            Spacing += "    ";
            break;
          case "}":
            Spacing = Spacing.Remove(0, 4);
            fRes.AppendLine(Spacing + "}");
            break;
          default:
            fRes.Append(Spacing);
            fRes.AppendLine(d);
            break;
        }
      }
      return fRes.ToString();
    }
    private static Regex mainLabel = new Regex("Init End");
    private static Regex mainLabelFinished = new Regex("Result Output Block Begin");
    private static Regex searchGoTo = new Regex("goto");
    private static Regex searchlLabel1 = new Regex("Slice1Label: ");
    private static Regex searchlLabel2 = new Regex("Slice1Labe2: ");
    private static Regex Slice1Label = new Regex("Slice1Label");
    private static Regex Slice2Label = new Regex("Slice2Label");
    private static Regex searchLabel = new Regex("label1");
    private static Regex searchLabel1 = new Regex("label2");
    private static Regex searchLabelRep = new Regex("label");
    private static Regex searchHold = new Regex("hold1");
    private static Regex searchHold2 = new Regex("hold2");
    //private static Regex searchlLabelFin = new Regex("lLabel:");
    private static Regex sekondSlice = new Regex("slice 2 begin");
    
   
    public string SetLabels(string s)
    {
      //string[] lines = splitter.Split(s);
      List<string> lines = s.SplitToLines();
      int i = 0;
      int i1 = 0;
      int iter = 0;
      int startNumber = 0;
      int stopNumber = 0;
      int machGoTo = 0;
      string Toggle = "";
      string lineTmp = "";
      string slices = "Slice1Label: "; 
      string slicesLabel = "label1";
      while (!mainLabel.IsMatch(lines[iter]))
      {
        iter++;
      }
      startNumber = iter + 1;
      i = startNumber;
      iter = startNumber;
      
      while (!mainLabelFinished.IsMatch(lines[iter]))
      {
        iter++;
      }
       
      stopNumber = iter;
      //foreach (string line in lines)
      for (int j = startNumber; j < stopNumber; j++)
      {
        lineTmp = lines[j].Trim();
        if (searchGoTo.IsMatch(lineTmp))
        {
          machGoTo++;
        }
        if (lineTmp.Length != 0)
        {
          Toggle = lineTmp[0].ToString();
        }
        else
        {
          Toggle = " ";
        }
        if (sekondSlice.IsMatch(lineTmp))
        {
          slices = "Slice2Label: ";
          slicesLabel = "label2";
        }
        lines[j] = searchLabelRep.Replace(lineTmp, slicesLabel);
        switch (Toggle)
        {
          case "{":
            lines[i] = "        " + lines[i];
            break;
          case "}":
            lines[i] = "        " + lines[i];
            break;
          case "c":
            lines[i] = "        " + lines[i];
            break;
          case "g":
            lines[i] = "        " + lines[i];
            break;
          case " ":
            lines[i] = "        " + lines[i];
            break;
          case "e":
            lines[i] = "        " + lines[i];
            break;
          case "/":
            lines[i] = "        " + lines[i];
            break;
          default:
            //lines[i] = "lLabel: " + lines[i];
            lines[i] = slices + lines[i];
            break;
        }
        
        i++;
      }
      //rndLabels = new ArrayList();
      //Random rndLab = new Random();
      //int tmpLab = rndLab.Next(0, machGoTo+1);
      //    for (int k = 0; k < machGoTo; k++)
      //    {
      //        while (rndLabels.Contains(tmpLab))
      //        {
      //            tmpLab = rndLab.Next(0, machGoTo + 1);
      //        }
      //        rndLabels.Add(tmpLab);
      //    }
      //for (int k = 0; k < machGoTo; k++)
      //{
      //    while (rndLabelsForReplacement.Contains(tmpLab))
      //    {
      //        tmpLab = rndLab.Next(0, machGoTo + 1);
      //    }
      //    rndLabelsForReplacement.Add(tmpLab);
      //}
      maximumLabels = machGoTo*2;
      string Res = lines.JoinLines();
      if (maximumLabels != 0)
      {
        Res = Slice1Label.Replace(Res.ToString(), replaceLabelLLabel1);
        Res = searchLabel.Replace(Res, replaceLabelIteration1);
        searchGotoForLabel2 = searchGotoForLabel1;
        Res = Slice2Label.Replace(Res, replaceLabelLLabel2);
        Res = searchLabel1.Replace(Res, replaceLabelIteration2);
        //Res = searchlLabel.Replace(Res, replaceLabelLLabel);
        //Res = searchLabel.Replace(Res, replaceLabelIteration);
        Res = searchHold.Replace(Res, "label");
        Res = searchHold2.Replace(Res, "label");
      }
      //Res = searchlLabelFin.Replace(Res, "");
      Res = Res.Replace("lLabel:", "");
      //lines = splitter.Split(Res);
      lines = Res.SplitToLines();
      string[] stringSeparators = new string[] { ": " };
      for (int j = startNumber; j < stopNumber; j++)
      {
        string[] strings = lines[j].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
        if (strings.Length > 1)
        {
          if (!_labelsArray.Contains(strings[0]))
          {
            lines[j] = "        " + lines[j].Substring(strings[0].Length + 1);
          }
        }

      }
      Res = lines.JoinLines();
      _labelsArray.Clear();
      return Res;
    }

    public int countGoTo = 0;
    public int searchGotoForLabel1 = 0;
    public int searchGotoForLabel2 = 0;
    public int maximumLabels = 0;
    public Random rndGo = new Random();
    private ArrayList _labelsArray = new ArrayList();

    private string replaceLabelIteration1(Match m)
    {
      string newLabel = "";
      int postfix = rndGo.Next(0, searchGotoForLabel1);
      newLabel = "label_" + postfix.ToString();
      _labelsArray.Add(newLabel);
      //newLabel = "label_" + countGoTo.ToString();
      countGoTo++;
      return newLabel;
    }

    private string replaceLabelLLabel1(Match m)
    {
      string newLabel = "";
      newLabel = "hold1_" + searchGotoForLabel1.ToString();
      searchGotoForLabel1++;
      return newLabel;
    }
    private string replaceLabelIteration2(Match m)
    {
      string newLabel = "";
      int postfix = rndGo.Next(searchGotoForLabel1, searchGotoForLabel2);
      newLabel = "label_" + postfix.ToString();
      _labelsArray.Add(newLabel);
      //newLabel = "label_" + countGoTo.ToString();
      countGoTo++;
      return newLabel;
    }

    private string replaceLabelLLabel2(Match m)
    {
      string newLabel = "";
      newLabel = "hold2_" + searchGotoForLabel2.ToString();
      searchGotoForLabel2++;
      return newLabel;
    }
  }
}