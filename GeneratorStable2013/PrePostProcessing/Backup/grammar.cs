 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace bnfGenerator
{
  internal class Grammar
  {
    public transductor mTransductor;
    public string[] mGrammarText;
    public static Dictionary<string, Rule> mRulls = new Dictionary<string, Rule>();
    public static List<Symbol> mSymbols = new List<Symbol>();
    private readonly Symbol mMainSymbol;

    public static Grammar FromTextFile(string aFileName)
    {
      List<string> lLines = new List<string>();
      using (StreamReader sr = new StreamReader(aFileName))
      {
        String line;
        while ((line = sr.ReadLine()) != null)
        {
          lLines.Add(line);
        }
      }
      String[] grammarLine = lLines.ToArray();
      return new Grammar(grammarLine);
    }

    public Grammar(string[] aGrammarText)
    {
      mGrammarText = aGrammarText;
      mSymbols.Clear();
      mRulls.Clear();
      Regex re = new Regex(@"\:\=");
      string[] mMainSymbolsString = re.Split(aGrammarText[0]);
      //string[] mMainSymbolsString = aGrammarText[0].Split('='); // ���������� ������� ������
      mMainSymbol = new Symbol(mMainSymbolsString[0].Trim() /*"�����������"*/, false);
      foreach (string s in aGrammarText)
      {
        // ������� �������.
        if (!string.IsNullOrEmpty(s))
        {
          Rule lRull = new Rule(s);
          //mRulls.Add(lRull.mLeftSide.mText, lRull);
          // ���������, ���� ���� ����� ������� � ���� ������, �� �� ��������� �������, � ��������� ������������
          if (mRulls.ContainsKey(lRull.mLeftSide))
          {
//���������� �������
            mRulls[lRull.mLeftSide].mRightSides.mAlternatives.AddRange(lRull.mRightSides.mAlternatives);
          }
          else
          {
            mRulls.Add(lRull.mLeftSide, lRull);
          }
        }
      }
      CheckTerminal();
    }

    private void CheckTerminal()
    {
      foreach (Symbol lSymbol in mSymbols)
      {
        if (lSymbol.mIsTerminal)
        {
          if (mRulls.ContainsKey(lSymbol.mText))
          {
            lSymbol.mIsTerminal = false;
          }
        }
      }
    }

    public List<Symbol> Generate()
    {
      return Expand(mMainSymbol, 0);
    }

    public List<Symbol> Expand(Symbol aSymbol, int count)
    {
      Symbol lSymbol = aSymbol;
      if (count > 2000)
      {
        throw new Exception("������� ������� ������� �����������. ���� �� ����� ���� �������������� ����������.");
      }
      if (aSymbol.mIsTerminal)
      {
        // ��������� �����
        Regex transReg = new Regex(@"\{([a-z]+)\}", RegexOptions.IgnoreCase);
        Regex placeReg = new Regex(@"\<([a-zA-Z]+)\>");
        Match lMatch = transReg.Match(aSymbol.mText);
        if (lMatch.Success)
        {
          //lSymbol = new Symbol(lMatch.Groups[1].Value, true);
          string transName = lMatch.Groups[1].Value;
          // ������� � ���� ������ ����� � ���� ������. ��������� ������������ ��� ������������ ������
          // ������ �����������. 
          MethodInfo lMethodInfo = mTransductor.GetType().GetMethod(transName);
          if (lMethodInfo != null)
          {
            // ����� ������ ������������
            object[] lTransParams = {aSymbol};
            string lFromTrans = (string) lMethodInfo.Invoke(mTransductor, lTransParams);
            lSymbol = new Symbol(lFromTrans, true);
          }
        }
        else
        {
          lMatch = placeReg.Match(aSymbol.mText);
          if (lMatch.Success)
          {
            mTransductor.addPlaceHolder(aSymbol);
          }
          else
          {
            // todo
          }
        }
        List<Symbol> lRes = new List<Symbol>();
        lRes.Add(lSymbol);
        return lRes;
      }
      else
      {
        // ������ �������
        if (mRulls.ContainsKey(aSymbol.mText))
        {
          Rule lRull = mRulls[aSymbol.mText];
          return Expand(lRull.GetAlternative().mSymbols, count + 1); //������������ - ������ ��������
        }
        else
        {
          // ����� ������. ������, ������� ������ ��������
          throw new Exception("�� ��������� �������������� ������");
        }
      }
    }

    public List<Symbol> Expand(List<Symbol> aSymbol, int count)
    {
      List<Symbol> lRes = new List<Symbol>();
      foreach (Symbol lSymbol in aSymbol)
      {
        lRes.AddRange(Expand(lSymbol, count)); // ��������� ���������� ������
      }
      return lRes;
    }
  }

  internal class Rule
  {
    private static readonly Random mRnd = new Random();

    public Rule(string aRullText)
    {
      // ����� � ������ ������� ����� split
      Regex reg = new Regex(":=");
      string[] lDoubleSide = reg.Split(aRullText); //.Split('=');
      if (lDoubleSide.Length < 2)
      {
        throw new Exception("������� � �������, �� �������� :=");
      }
      mLeftSide = lDoubleSide[0].Trim();
      mRightSides = new RightSide(lDoubleSide[1].Trim());
    }

    private int mCount = 0;

    public Alternative GetAlternative()
    {
      if (mLeftSide.EndsWith("s"))
      {
        int lMin = 0;
        int lMax = int.MaxValue;
        if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings[mLeftSide + "_min"]))
        {
          lMin = Convert.ToInt32(ConfigurationSettings.AppSettings[mLeftSide + "_min"]);
          lMax = Convert.ToInt32(ConfigurationSettings.AppSettings[mLeftSide + "_max"]);
          //  mRightSides.mAlternatives[0] - ����� ��������� (=stat)
          //  mRightSides.mAlternatives[1] - ����������� ��������� (=stats)
          mCount++;
          if (mCount < lMin)
          {
            return mRightSides.mAlternatives[1];
          }
          else if (mCount >= lMax)
          {
            return mRightSides.mAlternatives[0];
          }
        }
      }
      return GetRndAlternative();
    }

    public Alternative GetRndAlternative()
    {
      int i = mRnd.Next(mRightSides.mAlternatives.Count);
      return mRightSides.mAlternatives[i]; //������ ������������ //RND
    }

    public string mLeftSide;
    public RightSide mRightSides;
  }

  internal class RightSide
  {
    public List<Alternative> mAlternatives;

    public RightSide(string aRightSideText)
    {
      Regex r = new Regex("\\|");
      string[] lAltArray = r.Split(aRightSideText);
      //string[] lAltArray = aRightSideText.Split('|');
      mAlternatives = new List<Alternative>();
      foreach (string s in lAltArray)
      {
        mAlternatives.Add(new Alternative(s.Trim()));
      }
    }
  }

  internal class Alternative
  {
    public List<Symbol> mSymbols;

    public static string Print(List<Symbol> aSymbol)
    {
      string s = "";
      foreach (Symbol lSymbol in aSymbol)
      {
        s += lSymbol.mText + " ";
      }
      return s;
    }

    public Alternative(string aText)
    {
      string[] lSymbArray = aText.Split(',');
      mSymbols = new List<Symbol>();
      foreach (string s in lSymbArray)
      {
        string lText = s.Trim();
        Symbol lSymbol;
        //if (Grammar.mSymbols.ContainsKey(lText))
        // {
        //lSymbol = Grammar.mSymbols[lText]; // ���� ������, �� ����

        //} else
        {
          lSymbol = new Symbol(lText, true);
          Grammar.mSymbols.Add(lSymbol);
        }
        //symbol sSymbol = new symbol(s.Trim(), true);
        mSymbols.Add(lSymbol);
        // ���� �� ��� ��������������, �� ������ ����. ���� ������ � ������� ������
      }
    }
  }

  public class Symbol
  {
    public string mText;

    public Symbol(string aText, bool aIsTerminal)
    {
      mText = aText;
      mIsTerminal = aIsTerminal;
    }

    public bool mIsTerminal;
  }
}