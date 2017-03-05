 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Sanford.Collections.Generic;

namespace PrePostProcessing
{
  public class replaceHolding
  {
    public string inText = "";
    public string outText = "";
    private ArrayList indexes;
    private readonly int ArrayLength;
    private readonly int IndexCount;
    private readonly string fixedMinBound = "";
    private readonly string fixedMaxBound = "";
    private readonly string FixedCycleStep = "";
    public Random rnd = new Random();
    private readonly NormalDistribution rndND = new NormalDistribution(0, 2);

    public replaceHolding(string s, ArrayList ind, int arLength, int indCount)
    {
      inText = s;
      indexes = ind;
      ArrayLength = arLength;
      IndexCount = indCount;
      fixedMinBound = rnd.Next(10, 15).ToString();
      fixedMaxBound = rnd.Next(ArrayLength - 20, ArrayLength - 10).ToString();
      index_cntF = rnd.Next(0, 5);
      FixedCycleStep = rnd.Next(1, 3).ToString();
    }

    private string transText(int arLength)
    {
      Regex sep = new Regex(@" ");
      string[] tmp;
      string tempOut = "";
      string intConst = "rep_intConst";
      string realConst = "rep_realConst";
      string indND = "rep_indND";
      string absND = "rep_absND";
      string maxBound = "rep_maxBound";
      tmp = sep.Split(inText);
      for (int i = 0; i < tmp.Length; i++)
      {
        string r_intConst = rnd.Next(0, 100).ToString();
        string r_realConst = rnd.NextDouble().ToString();
        r_realConst = r_realConst.Substring(0, 4);
        r_realConst = r_realConst.Replace(",", ".");
        string r_indND = PlusMinus() + " " + Math.Abs(rndND.intND());
        string r_absND = Convert.ToString(Math.Abs(rndND.intND()));
        string r_maxBound = rnd.Next(arLength - 15, arLength - 10).ToString();
        tmp[i] = tmp[i].Replace(intConst, r_intConst);
        tmp[i] = tmp[i].Replace(realConst, r_realConst);
        tmp[i] = tmp[i].Replace(indND, r_indND);
        tmp[i] = tmp[i].Replace(absND, r_absND);
        tmp[i] = tmp[i].Replace(maxBound, r_maxBound);
      }
      for (int i = 0; i < tmp.Length; i++)
      {
        tempOut += tmp[i] + " ";
      }
      outText = tempOut.Substring(0, tempOut.Length - 1);
      inText = outText;
      return outText;
    }

    public string GetRootConfigDir()
    {
      string configDir = Program.WorkingDir + @"\ProcessingValidating\validating\";
      return configDir;
    }

    public void DefIndexes()
    {
      ArrayList defInds = new ArrayList();
      ArrayList defIntAr = new ArrayList();
      ArrayList defRealAr = new ArrayList();
      XmlDocument doc = new XmlDocument();
      doc.Load(GetRootConfigDir() + "members.xml");
      Regex reg = new Regex("(intIndex_\\d+ | realArray_\\d+ | intArray_\\d+ | realArraySS_\\d+ | intArraySS_\\d+)",
                            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
      MatchCollection mc = reg.Matches(inText);
      string s = "";
      foreach (Match m in mc)
      {
        s = m.Groups[0].Value;
        if (Regex.IsMatch(s, "intIndex"))
        {
          if (!defInds.Contains(s))
          {
            defInds.Add(s);
          }
        }
        if (Regex.IsMatch(s, "intArray"))
        {
          if (!defIntAr.Contains(s))
          {
            defIntAr.Add(s);
          }
        }
        if (Regex.IsMatch(s, "intArraySS"))
        {
          if (!defIntAr.Contains(s))
          {
            defIntAr.Add(s);
          }
        }
        if (Regex.IsMatch(s, "realArray"))
        {
          if (!defRealAr.Contains(s))
          {
            defRealAr.Add(s);
          }
        }
        if (Regex.IsMatch(s, "realArraySS"))
        {
          if (!defRealAr.Contains(s))
          {
            defRealAr.Add(s);
          }
        }
      }
      s = @"#include <stdio.h>" + "\r\n" + "\r\n" + "\r\n";
      //s = " ";
      string initInds = "";
      string initIntArrays = "";
      string initRealArrays = "";
      string outInds = "";
      string outIntArrays = "";
      string outRealArrays = "";
      XmlNode type;
      type = doc.SelectSingleNode("/supportedItems/C/definitions/intIndex/@type");
      for (int i = 0; i < defInds.Count; i++)
      {
        s += type.Value + " " + defInds[i].ToString() + "; ";
        initInds += @"if( fscanf(input, ""%d"", &" + defInds[i] + @") == EOF )
				return;";
        outInds += @"fprintf(output, ""%d\n"", " + defInds[i] + @");";
      }
      type = doc.SelectSingleNode("/supportedItems/C/definitions/intArray/@type");
      for (int i = 0; i < defIntAr.Count; i++)
      {
        s += type.Value + " " + defIntAr[i].ToString() + "[" + ArrayLength + "]; ";
        initIntArrays += @"for( __i__ = 0 rep_sep __i__ < " + ArrayLength + @" rep_sep __i__++)
			{
				if( fscanf(input, ""%lf"", &" + defIntAr[i] + @"[ __i__ ]) == EOF || feof(input) )
					return;
			}";
        outIntArrays += @"for( __i__ = 0 rep_sep __i__ < " + ArrayLength + @" rep_sep __i__++)
      {
        fprintf(output, ""%lf\t"", " + defIntAr[i] + @"[ __i__ ]);
      }";
      }
      type = doc.SelectSingleNode("/supportedItems/C/definitions/realArray/@type");
      for (int i = 0; i < defRealAr.Count; i++)
      {
        s += type.Value + " " + defRealAr[i].ToString() + "[" + ArrayLength + "]; ";
        initRealArrays += @"for( __i__ = 0 rep_sep __i__ < " + ArrayLength + @" rep_sep __i__++)
			{
				if( fscanf(input, ""%lf"", &" + defRealAr[i] + @"[ __i__ ]) == EOF || feof(input) )
					return;
			}";
        outIntArrays += @"for( __i__ = 0 rep_sep __i__ < " + ArrayLength + @" rep_sep __i__++)
      {
        fprintf(output, ""%lf\t"", " + defRealAr[i] + @"[ __i__ ]);
      }";
      }
      inText = Regex.Replace(inText, "replaceVars", s);
      // Вставка инициализации
      string blockInitFirst = @"
      		
			FILE* input;
			FILE* output;
			int __i__;
			
			input = fopen(""input.txt"", ""r"");
			if (!input)
			  return;
      " + "\r\n";
      string initBegin = "";
      string initEnd = "";
      string blockInitLast = @"/* Result Output Block Begin */" + "\r\n" + @"
	
        output = fopen(""output.txt"", ""w"");
        if (!output)
        return;
      ";
      if (initInds.Length > 1 || initIntArrays.Length > 1 || initRealArrays.Length > 1)
      {
        initBegin += @"/* Init Begin */" + "\r\n" + blockInitFirst + initInds + initIntArrays + initRealArrays +
                     @" fclose(input); " + "\r\n" + @"/* Init End */" + "\r\n";
        initEnd += blockInitLast + outInds + outIntArrays + outRealArrays + @" fclose(output); " + "\r\n" +
                   @"/* Result Output Block End */" + "\r\n";
       // выключить блок заполнения
        //initBegin = @"
         //Init Begin 
         //Init End 
      //";
       // initEnd = @"/* Result Output Block Begin */" + "\r\n" + @"
         /*Result Output Block End */ //" + "\r\n";
        inText = Regex.Replace(inText, "rep_InitBegin", initBegin);
        outText = Regex.Replace(inText, "rep_InitEnd", initEnd);
      }
      // Конец вставки инициализации

      // Подстановка выражений - начало

      Match bes_r = Regex.Match(outText, "/\\*BES-r_begin\\*/(?<besr>.+?); /\\*BES-r_end\\*/", RegexOptions.None);
      string realSubList = rnd.Next(0, 100).ToString();
      List<string> subReal = new List<string>();
      while (bes_r.Success)
      {
          Group besRRes = bes_r.Groups["besr"];
          subReal.Add(besRRes.Value);
          if (besRRes.Value.Length > realSubList.Length) realSubList = besRRes.Value;
          bes_r = bes_r.NextMatch();
      }
      //realSubList = new List<string>(Regex.Split(realSubList[0], " ; /*BES-r_end*/ /*BES-i_begin*/ "));
      Match bes_i = Regex.Match(outText, "/\\*BES-i_begin\\*/(?<besi>.+?); /\\*BES-i_end\\*/", RegexOptions.None);
      string intSubList = rnd.Next(0, 100).ToString();
      List<string> subInt = new List<string>();
      while (bes_i.Success)
      {
        Group besIRes = bes_i.Groups["besi"];
        subInt.Add(besIRes.Value);
        if (besIRes.Value.Length > intSubList.Length) intSubList = besIRes.Value;
        bes_i = bes_i.NextMatch();
      }
      if (subInt.Count == 0)
      {
        outText = Regex.Replace(outText, "substitution-i", intSubList, RegexOptions.IgnoreCase);
      }
      else
      {
        outText = Regex.Replace(outText, "substitution-i", delegate
          {
            return subInt[rnd.Next(0, subInt.Count - 1)];
          }, RegexOptions.IgnoreCase);
      }
      if (subReal.Count == 0)
      {
        outText = Regex.Replace(outText, "substitution-r", realSubList, RegexOptions.IgnoreCase);
      }
      else
      {
        outText = Regex.Replace(outText, "substitution-r", delegate
        {
          return subReal[rnd.Next(0, subReal.Count - 1)];
        }, RegexOptions.IgnoreCase);
      }
      // очистить заглушки
      //outText = Regex.Replace(outText, "/\\*BES-i_begin\\*/(?<besi>.+?); /\\*BES-i_end\\*/", "");
      //outText = Regex.Replace(outText, "/\\*BES-r_begin\\*/(?<besr>.+?); /\\*BES-r_end\\*/", "");
      outText = Regex.Replace(outText, "/\\*BES-i_begin\\*/", "");
      outText = Regex.Replace(outText, "/\\*BES-i_end\\*/", "");
      outText = Regex.Replace(outText, "/\\*BES-r_begin\\*/", "");
      outText = Regex.Replace(outText, "/\\*BES-r_end\\*/", "");
      // Подстановка выражений - конец
      inText = outText;
      //return s;
    }
    private int realPlacesCount = 0;
    private int intPlacesCount = 0;
    private int realPlacesStartCounter = 0;
    private int intPlacesStartCounter = 0;
    private List<string> usedLinkedVarsList = new List<string>();
    public List<objVars> realVarsLinked = new List<objVars>();
    public List<objVars> intVarsLinked = new List<objVars>();
    public class objVars
    {
      public int mCount;
      public string mName;
    }
    private void TransformConstants()
    {
      string s = "";
      Regex reg =
        new Regex(
          "( realArrayRep | realArrayFreeRep | block_end | block_begin | intArrayRep | intArrayFreeRep | rep_intConst | rep_realConst |rep_NFreeForm| rep_minBound | rep_maxBound |rep_minFBound | rep_maxFBound | rep_indND | rep_absND | rep_FStep )",
          RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      Regex realPlaces = new Regex("realArrayRep", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      Regex intPlaces = new Regex("intArrayRep", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

      Regex intFreeVarPlaces = new Regex("intArrayFreeRep", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      Regex realFreeVarPlaces = new Regex("intArrayFreeRep", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      intFreeMathesCounter = intFreeVarPlaces.Matches(inText).Count;
      realFreeMathesCounter = realFreeVarPlaces.Matches(inText).Count;
      realPlacesCount = realPlaces.Matches(inText).Count;
      intPlacesCount = intPlaces.Matches(inText).Count;
      currentmpBlockUsedVars.Clear();

      //Debug.Assert(realPlacesCount > 0);
      if (realPlacesCount > 0)
      {
        FillLinkedVars(0, realPlacesCount / 3, realVarsLinked, "realArray");
      }
      if (intPlacesCount > 0)
      {
        FillLinkedVars(0, intPlacesCount / 3, intVarsLinked, "intArray");
      }
      if (intFreeMathesCounter > 0)
      {
        FillFreeVarsBlock("intArray");
      }
      if (realFreeMathesCounter > 0)
      {
        FillFreeVarsBlock("realArray");
      }
      string tmp = reg.Replace(inText, replaceConsts);
      s = tmp;
      outText = s;
      inText = outText;
      //return tmp;
    }
    private int intFreeMathesCounter = 0;
    private int realFreeMathesCounter = 0;
    private List<string> currenBlockRealVars = new List<string>();
    private List<string> currenBlockIntVars = new List<string>();
    private HashSet<string> currenBlockUsedVars = new HashSet<string>();
    private HashSet<string> currentmpBlockUsedVars = new HashSet<string>();
    private void FillFreeVarsBlock(string mName)
    {
      int currentBlockCount = 0;
      int tmpCounter = 0;
      int ti = 0;
      if (mName == "intArray")
      {

        while (tmpCounter < 5 || ti < 100)
        {
          currentBlockCount = rnd.Next(intPlacesCount, intPlacesCount + intFreeMathesCounter);
          string tmpName = mName + "_" + currentBlockCount;
          if (!currenBlockUsedVars.Contains(tmpName))
          {
            currenBlockIntVars.Add(tmpName);
            tmpCounter++;
          }
          ti++;
        }
      }
      else
      {

        while (tmpCounter < 10)
        {
          currentBlockCount = rnd.Next(realPlacesCount, realPlacesCount + realFreeMathesCounter);
          string tmpName = mName + "_" + currentBlockCount;
          if (!currenBlockUsedVars.Contains(tmpName))
          {
            currenBlockRealVars.Add(tmpName);
            tmpCounter++;
          }
        }
      }
    }

    private void FillLinkedVars(int startCounter, int EndCounter, List<objVars> mrealVarsLinked, string mName)
    {
      for (int i = startCounter; i <= EndCounter; i++)
      {
        objVars currentVar = new objVars();
        currentVar.mCount = rnd.Next(0, 10);
        currentVar.mName = mName + "_" + i;
        mrealVarsLinked.Add(currentVar);
      }
      if (mName == "realArray")
      {
        realPlacesStartCounter = EndCounter;
      }
      else
      {
        intPlacesStartCounter = EndCounter;
      }
    }


    private string replaceConsts(Match m)
    {
      string res = "";
      switch (m.Groups[0].Value)
      {
        // TODO Проработать генерацию зависимостей
        case "realArrayRep":
          //res = "realArray_" + rnd.Next(0, 20).ToString() + "[ comInd rep_indND ]";
          res = SetVarName(res, realVarsLinked, "realArray");

          break;
        case "block_end":
          //res = "block end" + rnd.Next(0, 20).ToString() + "[ comInd rep_indND ]";
          currenBlockIntVars.Clear();
          currenBlockRealVars.Clear();
          foreach (string usedNames in currentmpBlockUsedVars)
          {
            currenBlockUsedVars.Add(usedNames);
          }
          FillFreeVarsBlock("intArray");
          FillFreeVarsBlock("realArray");
          res = "block end";
          break;
        case "block_begin":
          res = "block begin";
          break;
        case "intArrayRep":
          //res = "intArray_" + rnd.Next(0, 20).ToString() + "[ comInd rep_indND ]";
          res = SetVarName(res, intVarsLinked, "intArray");
          break;
        case "intArrayFreeRep":
          bool Found = false;
          string curIntVar = "";
          while (!Found)
          {
            curIntVar = currenBlockIntVars[rnd.Next(0, currenBlockIntVars.Count)].ToString();
            if (!currenBlockUsedVars.Contains(curIntVar))
            {
              Found = true;
            }
          }
          res = curIntVar + "[ comInd rep_indND ]";
          currentmpBlockUsedVars.Add(curIntVar);
          break;
        case "realArrayFreeRep":
          bool mFound = false;
          string mcurIntVar = "";
          while (!mFound)
          {
            if (currenBlockRealVars.Count > 0)
            {
              mcurIntVar = currenBlockRealVars[rnd.Next(0, currenBlockRealVars.Count)].ToString();
              if (!currenBlockUsedVars.Contains(mcurIntVar))
              {
                mFound = true;
              }
            }
            else
            {
              break;
            }
          }
          res = mcurIntVar + "[ comInd rep_indND ]";
          currentmpBlockUsedVars.Add(mcurIntVar);
          break;
        case "rep_intConst":
          res = rnd.Next(0, 100).ToString();
          break;
        case "rep_NFreeForm":
          res = rnd.Next(0, 1000).ToString();
          break;
        case "rep_realConst":
          double realNumber = rnd.NextDouble()*rnd.Next(1, 1000);
          string restmp = realNumber.ToString();
          res = restmp.Replace(",", ".");
          break;
        case "rep_minBound":
          res = rnd.Next(15, 20).ToString();
          break;
        case "rep_minFBound":
          res = fixedMinBound; // фиксирование для слияния
          break;
        case "rep_maxBound":
          res = rnd.Next(ArrayLength - 35, ArrayLength - 25).ToString();
          break;
        case "rep_maxFBound":
          res = fixedMaxBound; // фиксирование для слияния
          break;
        case "rep_indND":
          res = PlusMinus() + " " + Math.Abs(rndND.intND()).ToString();
          break;
        case "rep_absND":
          int tmp = Math.Abs(rndND.intND());
          if (tmp == 0)
          {
            res = "1";
          }
          else
          {
            res = tmp.ToString();
          }
          break;
        case "rep_FStep":
          res = FixedCycleStep;
          break;
      }
      return res;
    }

    private string SetVarName(string res, List<objVars> mrealVarsLinked, string mName)
    {
      bool selectFinished = false;
      while (!selectFinished)
      {
        if (mrealVarsLinked.Count < 2)
        {
          FillLinkedVars(realPlacesStartCounter, realPlacesStartCounter + 5, mrealVarsLinked, mName);
        }
        int currentIndex = rnd.Next(0, mrealVarsLinked.Count);
        if (mrealVarsLinked[currentIndex].mCount >= -10)
        {
          string localPlusMinus = "";
          if (mrealVarsLinked[currentIndex].mCount >= 0)
          {
            localPlusMinus = "+";
          }
          else
          {
            localPlusMinus = "-";
          }
          res = mrealVarsLinked[currentIndex].mName + "[ comInd " + localPlusMinus + " " +
                Math.Abs(mrealVarsLinked[currentIndex].mCount) + " ]";
          mrealVarsLinked[currentIndex].mCount--; // = realVarsLinked[currentIndex].mCount + 1;
          if (!usedLinkedVarsList.Contains(mrealVarsLinked[currentIndex].mName))
          {
            usedLinkedVarsList.Add(mrealVarsLinked[currentIndex].mName);
          }
          selectFinished = true;
        }
        else
        {
          mrealVarsLinked.RemoveAt(currentIndex);
        }
      }
      return res;
    }

    private void RemoveEmptyBodies()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(GetRootConfigDir() + "members.xml");
      XmlNode state = doc.SelectSingleNode("/supportedItems/C/statements");
      XmlNodeList bodies = state.ChildNodes;
      foreach (XmlNode body in bodies)
      {
        string bodyName = body.Name.ToString() + body.Name.ToString();
        inText = inText.Replace(bodyName, "");
      }
      outText = inText;
    }

    public string PrintText()
    {
      Formatter myFormat = new Formatter();
      TransformConstants();
      ForProcessing();
      DefIndexes();
      RemoveEmptyBodies();
      inText = inText.Replace("void main", "\r\nvoid main");
      inText = inText.Replace("{", "\r\n{\r\n");
      inText = inText.Replace("/*", "\r\n/*");
      inText = inText.Replace("*/", "*/\r\n");
      inText = inText.Replace("}", "\r\n}\r\n");
      inText = inText.Replace(";", ";\r\n");
      inText = inText.Replace("rep_sep", ";");
      inText = Formatter.Format(inText);
      inText = myFormat.SetLabels(inText);
      outText = inText;
      return outText;
    }

    private string PlusMinus()
    {
      string[] sign = new string[2] { "-", "+" };
      return sign[rnd.Next(0, 2)];
    }

    private void ForProcessing()
    {
      TransformConstants();
      string s = "";
      mStack.Clear();
      deqIndex.Clear();
      Regex reg = new Regex("( continue\\s; | break\\s; | forInd | forFInd | comInd | whileInd | { | } )",
                            RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      string tmp = reg.Replace(inText, replaceIndexes);
      s = tmp;
      outText = s;
      inText = outText;
      //return outText;
    }

    private string mPrevMatch = "";
    private Stack<string> mBoundStack = new Stack<string>();
    private readonly Stack<string> mStack = new Stack<string>();
    private readonly Deque<string> deqIndex = new Deque<string>();
    private int find_cntFor = 0;
    private int find_cntWhile = 0;
    private int index_cnt = 0;
    private readonly int index_cntF = 0;
    private string mCurrentFind;
    private bool findCycle = false;

    private string replaceIndexes(Match m)
    {
      string res = "";
      string idx = "intIndex_";
      int indCount = 0;
      int newIndex;

      switch (m.Groups[0].Value)
      {
        case "comInd":
          if (deqIndex.Count != 0)
          {
            if (indCount != IndexCount)
            {
              newIndex = 1; //rnd.Next(0, 2);
            }
            else
            {
              newIndex = 0;
            }

            switch (newIndex)
            {
              case 0:
                deqIndex.PeekFront();
                break;
              case 1:
                idx = idx + index_cnt++.ToString();
                deqIndex.PushBack(idx);
                indCount++;
                break;
            }
          }
          else
          {
            idx = idx + index_cnt++.ToString();
            deqIndex.PushBack(idx);
          }
          int cPop = Math.Abs(rndND.intND());
          while (cPop >= deqIndex.Count)
          {
            cPop = Math.Abs(rndND.intND());
          }
          res = deqIndex.ToArray().GetValue(cPop).ToString();
          break;
        case "forInd":
          //push new index to stack
          if (find_cntFor == 0)
          {
            idx = idx + index_cnt++.ToString();
            deqIndex.PushFront(idx);
            mCurrentFind = deqIndex.PeekFront();
          }
          find_cntFor++;
          if (find_cntFor == 4) find_cntFor = 0;
          res = mCurrentFind;
          break;
        case "forFInd":
          //push new index to stack
          if (find_cntFor == 0)
          {
            idx = idx + index_cntF.ToString(); // фиксирование заголовка цикла для слияния
            deqIndex.PushFront(idx);
            mCurrentFind = deqIndex.PeekFront();
          }
          find_cntFor++;
          if (find_cntFor == 4) find_cntFor = 0;
          res = mCurrentFind;
          break;
        case "whileInd":
          //push new index to stack

          if (find_cntWhile == 0)
          {
            idx = idx + index_cnt++.ToString();

            deqIndex.PushFront(idx);
            mCurrentFind = deqIndex.PeekFront();
          }
          find_cntWhile++;
          if (find_cntWhile == 4) find_cntWhile = 0;
          res = mCurrentFind;
          break;
        case "{":
          if (mPrevMatch == "whileInd")
          {
            mStack.Push("f{");
          }
          else if
            (mPrevMatch == "forInd")
          {
            mStack.Push("f{");
          }
          else
          {
            mStack.Push("{");
          }
          res = "{";
          break;
        case "}":
          if (mStack.Pop() == "f{")
          {
            //remove index from stack
            //mIndexStack.Pop();
            deqIndex.PopFront();
          }
          res = "}";
          break;
        case "break ;":
          if (mPrevMatch == "break ;")
          {
            res = "";
          }
          else
          {
            res = "break ;";
          }
          break;
        case "continue ;":
          if (mPrevMatch == "continue ;")
          {
            res = "";
          }
          else
          {
            res = "continue ;";
          }
          break;
      }
      mPrevMatch = m.Groups[0].Value;
      return res;
    }

    private string SmartIndex()
    {
      string s = "";
      //mIndexStack.Clear();
      mStack.Clear();
      deqIndex.Clear();
      string test =
        @"a[ind] = b[ind] + d[ind];
            for (find = 0; find <= 34; find + 1)
            {
                c[ind] = a[ind];
                if (a[ind] < 23)
                    {
                        c[ind] = a[ind];
                        c[ind] = a[ind];
                    }
                for (find = 0; find <= 34; find + 1)
                {
                    c[ind] = a[ind];
                    c[ind] = a[ind];
                }
                c[ind] = a[ind];
                c[ind] = a[ind];
            }
            for (find = 0; find <= 34; find + 1)
            {
                c[ind] = a[ind];
                c[ind] = a[ind];
            }
            c[ind] = a[ind]+r[ind];
            ";
      Regex reg = new Regex("( find | ind | { | } )", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      string tmp = reg.Replace(test, OurFunc);
      s = tmp;
      return s;
    }

    private string OurFunc(Match m)
    {
      //Random rnd = new Random();
      string res = "";
      char idx;
      int alt = 0;
      //deqIndex.
      //Random newIndex = new Random();
      switch (m.Groups[0].Value)
      {
        case "ind":
          //res = rnd.Next(0, 50).ToString();
          if ( /*mIndexStack.Count*/ deqIndex.Count != 0)
          {
            int newIndex = rnd.Next(0, 2);
            switch (newIndex)
            {
              case 0:
                //mIndexStack.Peek();
                deqIndex.PeekFront();
                break;
              case 1:
                //idx = (char)((int)'a' + mIndexStack.Count);
                //mIndexStack.Push(idx.ToString());
                idx = (char)((int)'a' + deqIndex.Count);
                deqIndex.PushBack(idx.ToString());
                break;
            }
          }
          else
          {
            //idx = (char)((int)'a' + mIndexStack.Count);
            //mIndexStack.Push(idx.ToString());
            idx = (char)((int)'a' + deqIndex.Count);
            deqIndex.PushBack(idx.ToString());
          }
          //res = mIndexStack.Peek();
          int cPop = Math.Abs(rndND.intND());
          while (cPop >= deqIndex.Count)
          {
            cPop = Math.Abs(rndND.intND());
          }
          res = deqIndex.ToArray().GetValue(cPop).ToString();
          //res = deqIndex.PeekFront();
          break;
        case "find":
          //push new index to stack
          if (find_cntFor == 0)
          {
            //idx = (char)((int)'i' + mIndexStack.Count);
            //mIndexStack.Push("f" + idx);
            //mCurrentFind = mIndexStack.Peek();
            idx = (char)((int)'i' + deqIndex.Count);
            deqIndex.PushFront("f" + idx);
            mCurrentFind = deqIndex.PeekFront();
          }
          find_cntFor++;
          if (find_cntFor == 3) find_cntFor = 0;
          res = mCurrentFind;
          break;
        case "{":
          if (mPrevMatch == "find")
          {
            mStack.Push("f{");
          }
          else
          {
            mStack.Push("{");
          }
          res = "{";
          break;
        case "}":
          if (mStack.Pop() == "f{")
          {
            //remove index from stack
            //mIndexStack.Pop();
            deqIndex.PopFront();
          }
          res = "}";
          break;
      }
      mPrevMatch = m.Groups[0].Value;
      return res;
    }
  }
}