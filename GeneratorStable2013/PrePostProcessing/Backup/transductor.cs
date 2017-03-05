 
using System;
using System.Collections;
using System.Collections.Generic;

namespace bnfGenerator
{
  public class VarDefinition
  {
    private readonly string mName;

    public string Name
    {
      get { return mName; }
    }

    public readonly int mRang;
    public int[] mSize;

    public VarDefinition(string aName, int aRang)
    {
      mName = aName;
      mRang = aRang;
      mSize = new int[mRang];
    }

    public override string ToString()
    {
      return mName + "[" + mSize[0] + "]";
    }
  }

  public class transductor
  {
    private int mCount = 0;
    private string mIndex = "";
    private int mCountIndex = 0;
    private int mMinBound = 0;
    private int mMaxBound = 0;
    private readonly Random mRnd = new Random();
    private readonly ArrayList mVarBlock = new ArrayList();
    private readonly Dictionary<string, Symbol> mPlaceHolders = new Dictionary<string, Symbol>();

    public string var(Symbol aSymbol)
    {
      VarDefinition lVarDef = (VarDefinition) mVarBlock[mRnd.Next(0, mVarBlock.Count)];
      if (mMaxBound > lVarDef.mSize[0])
      {
        mMaxBound = lVarDef.mSize[0];
      }
      setPlaceHolder("maxBound", mMaxBound.ToString());
      return lVarDef.Name;
    }

    public string vardev(Symbol aSymbol)
    {
      //string lName = "mas_" + char.ConvertFromUtf32('a' + mCount++).ToString();
      string lName = "mas_" + mCount.ToString();
      mCount++;
      int lRang = 1;
      VarDefinition lVarDef = new VarDefinition(lName, lRang);
      lVarDef.mSize[0] = mRnd.Next(99, 150);
      mVarBlock.Add(lVarDef);
      return lVarDef.ToString();
    }

    public string inddev(Symbol aSymbol)
    {
      mIndex = char.ConvertFromUtf32('a' + mCountIndex++).ToString();
      mMinBound = 0;
      mMaxBound = int.MaxValue;
      return mIndex;
    }

    public string ind(Symbol aSymbol)
    {
      return mIndex;
    }


    public string nconst(Symbol aSymbol)
    {
      //int m = Conf.GetIntDef("const_mean", 0);
      //int v = Conf.GetIntDef("const_variance", 1);
      //RandGenNormal lRnd = new RandGenNormal(m, v);
      //int i = (int)lRnd.Next();
      //return i.ToString();
      return "0";
    }

    public void addPlaceHolder(Symbol aSymbol)
    {
      if (mPlaceHolders.ContainsKey(aSymbol.mText))
      {
        mPlaceHolders[aSymbol.mText] = aSymbol;
      }
      else
      {
        mPlaceHolders.Add(aSymbol.mText, aSymbol);
      }
    }

    public void setPlaceHolder(string aName, string aValue)
    {
      aName = "<" + aName + ">";
      if (mPlaceHolders.ContainsKey(aName))
      {
        mPlaceHolders[aName].mText = aValue;
      }
    }
  }
}