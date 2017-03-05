 
using System;
using System.Collections.Generic;
using GrammarCompiler;
using Tools;

namespace GrammarTransductors
{
  public class SysTransductor
  {
    #region Fields, .ctor

    public DerivationContext Context;
    private static Dictionary<string, int> mNameCountDic;
    private static Random rnd;

    public SysTransductor()
    {
      mNameCountDic = new Dictionary<string, int>();
      rnd = new Random();
    }

    #endregion

    #region Rnd, Let

    public int Rnd(int max)
    {
      return rnd.Next(max);
    }

    public int Rnd(int min, int max)
    {
      return rnd.Next(min, max);
    }

    public string Let()
    {
      return Let("a", "z");
    }

    public string Let(string aFrom, string aTo)
    {
      int lFrom = char.ConvertToUtf32(aFrom, 0);
      int lTo = char.ConvertToUtf32(aTo, 0);
      int lCode = rnd.Next(lFrom, lTo + 1);
      return char.ConvertFromUtf32(lCode);
    }

    public string Norm(double mean, double sigma)
    {
      NormalDistribution nd = new NormalDistribution(mean, sigma);
      return nd.intND().ToString();
    }

    #endregion

    public string NameCount(string aPreffix)
    {
      int lCount;
      if (mNameCountDic.ContainsKey(aPreffix))
      {
        lCount = ++mNameCountDic[aPreffix];
      }
      else
      {
        lCount = 1;
        mNameCountDic.Add(aPreffix, lCount);
      }
      return string.Format("{0}{1}", aPreffix, lCount);
    }


    public void Clear()
    {
      mNameCountDic.Clear();
    }

    public string Hello(int a)
    {
      return new string('a', a);
    }

    public string Hello(string s)
    {
      return "hello " + s + " bye";
    }

    public string TypeOf(object o)
    {
      return o.GetType().Name;
    }

    public IDerivation RndItem(ListDerivation aList)
    {
      int i = rnd.Next(aList.mList.Count);
      return aList[i];
    }

    public ListDerivation ListAdd(IDerivation aList1Der, IDerivation aList2Der)
    {
      ListDerivation lList1 = aList1Der as ListDerivation;
      ListDerivation lList2 = aList2Der as ListDerivation;
      ListDerivation lResult = new ListDerivation(true, Context);
      lResult.mList.AddRange(lList1.mList);
      lResult.mList.AddRange(lList2.mList);
      return lResult;
    }
  }


//  public class VarDefinition
//  {
//    public readonly int mRang;
//    private string mName;
//    public int[] mSize;

//    public VarDefinition(string aName, int aRang)
//    {
//      mName = aName;
//      mRang = aRang;
//      mSize = new int[mRang];
//    }

//    public string Name
//    {
//      get { return mName; }
//    }

//    public override string ToString()
//    {
//      return mName + "[" + mSize[0] + "]";
//    }
//  }

//  public class transductor
//  {
//    private int mCount = 0;
//    private int mCountIndex = 0;
//    private string mIndex = "";
//    private int mMaxBound = 0;
//    private int mMinBound = 0;
//    private Dictionary<string, Symbol> mPlaceHolders = new Dictionary<string, Symbol>();
//    private Random mRnd = new Random();
//    private ArrayList mVarBlock = new ArrayList();

//    public string var(Symbol aSymbol)
//    {
//      VarDefinition lVarDef = (VarDefinition) mVarBlock[mRnd.Next(0, mVarBlock.Count)];
//      if (mMaxBound > lVarDef.mSize[0])
//      {
//        mMaxBound = lVarDef.mSize[0];
//      }
//      setPlaceHolder("maxBound", mMaxBound.ToString());
//      return lVarDef.Name;
//    }

//    public string vardev(Symbol aSymbol)
//    {
//      //string lName = "mas_" + char.ConvertFromUtf32('a' + mCount++).ToString();
//      string lName = "mas_" + mCount.ToString();
//      mCount++;
//      int lRang = 1;
//      VarDefinition lVarDef = new VarDefinition(lName, lRang);
//      lVarDef.mSize[0] = mRnd.Next(99, 150);
//      mVarBlock.Add(lVarDef);
//      return lVarDef.ToString();
//    }

//    public string inddev(Symbol aSymbol)
//    {
//      mIndex = char.ConvertFromUtf32('a' + mCountIndex++).ToString();
//      mMinBound = 0;
//      mMaxBound = int.MaxValue;
//      return mIndex;
//    }

//    public string ind(Symbol aSymbol)
//    {
//      return mIndex;
//    }


  ///*
//    public string nconst(Symbol aSymbol)
//    {
//      int m = Conf.GetIntDef("const_mean", 0);
//      int v = Conf.GetIntDef("const_variance", 1);
//      RandGenNormal lRnd = new RandGenNormal(m, v);
//      int i = (int) lRnd.Next();
//      return i.ToString();
//    }
//*/

//    public void addPlaceHolder(Symbol aSymbol)
//    {
//      if (mPlaceHolders.ContainsKey(aSymbol.Text))
//      {
//        mPlaceHolders[aSymbol.Text] = aSymbol;
//      }
//      else
//      {
//        mPlaceHolders.Add(aSymbol.Text, aSymbol);
//      }
//    }

//    public void setPlaceHolder(string aName, string aValue)
//    {
//      aName = "<" + aName + ">";
//      if (mPlaceHolders.ContainsKey(aName))
//      {
//        mPlaceHolders[aName].Text = aValue;
//      }
//    }
//  }
}