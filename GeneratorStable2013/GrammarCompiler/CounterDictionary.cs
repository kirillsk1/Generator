 
using System.Collections.Generic;

namespace GrammarCompiler
{
  /// <summary>
  /// Представляет собой мульти-индекс или "число" или составной счетчик для перебора, цифрами которого выступают экземпляры Counter
  /// </summary>
  public class CounterDictionary
  {
    //public bool IsCounterIncreased;
    private readonly Dictionary<string, Counter> mDic = new Dictionary<string, Counter>();
    private readonly List<string> mList = new List<string>();
    //private int mPointer = 0;
    //private Counter mLastCounter = null;
    //public bool IsLastCounter(Counter aCnt)
    //{
    //  return mLastCounter == aCnt;
    //}

    public Counter FindOrAddCounter(string aName, int aMaxValue)
    {
      if (mDic.ContainsKey(aName))
      {
        return mDic[aName];
      }
      else
      {
//add
        Counter lCounter = new Counter(aMaxValue);
        mDic.Add(aName, lCounter);
        mList.Add(aName);
        //mPointer = mList.Count - 1;
        return lCounter;
      }
    }

    /// <summary>
    /// Увеличивает мильтииндекс ("число" перебора") на единицу
    /// </summary>
    /// <returns>true - переполнение "числа" значит перебор завершен</returns>
    public bool Increase()
    {
      if (mDic.Count > 0)
      {
        return IncreaseRec(0);
      }
      return false;
    }

    private bool IncreaseRec(int aPointer)
    {
      string lName = mList[aPointer];
      Counter lCounter = mDic[lName];
      if (lCounter.Increase())
      {
//overflow
        if (aPointer >= mDic.Count - 1)
        {
          return true; // throw new GrammarCounterEndException();
        }
        return IncreaseRec(aPointer + 1);
      }
      return false;
    }
  }

  public class Counter
  {
    private int mValue;
    private readonly int mMaxValue;

    public int Value
    {
      get { return mValue; }
    }

    public Counter(int aMaxValue)
    {
      mValue = 0;
      mMaxValue = aMaxValue;
    }

    /// <summary>
    /// Increases this counter and check overflow
    /// </summary>
    /// <returns>ture indicates overflow (need to inc next counter)</returns>
    public bool Increase()
    {
      mValue += 1;
      if (mValue >= mMaxValue)
      {
        mValue = 0; // дать сигнал след. счетчику, что он будет увеличиваться
        return true;
      }
      return false;
    }
  }
}