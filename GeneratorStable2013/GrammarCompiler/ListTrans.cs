 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tools;

namespace GrammarCompiler
{
  public class ListTrans
  {
    public ListDerivation TargetList;
    public string TargetListName;

    public int Count()
    {
      return TargetList.mList.Count;
    }

    public IDerivation AddBegin(IDerivation aValue)
    {
      TargetList.mList.Insert(0, aValue);
      return aValue;
    }

    public IDerivation AddBegin(string aValue)
    {
      return AddBegin(new TextDerivation(aValue));
    }

    public IDerivation AddBegin(int aValue)
    {
      return AddBegin(new TextDerivation(aValue.ToString()));
    }


    public IDerivation AddEnd(IDerivation aValue)
    {
      TargetList.Add(aValue);
      return aValue;
    }

    public IDerivation AddEnd(string aValue)
    {
      return AddEnd(new TextDerivation(aValue));
    }

    public IDerivation AddEnd(int aValue)
    {
      return AddEnd(new TextDerivation(aValue.ToString()));
    }

    public IDerivation PopBegin()
    {
      CheckIfEmpty();
      IDerivation lRes = TargetList.mList[0];
      TargetList.mList.RemoveAt(0);
      return lRes;
    }

    public IDerivation PopEnd()
    {
      CheckIfEmpty();
      IDerivation lRes = TargetList.mList[TargetList.mList.Count - 1];
      TargetList.mList.RemoveAt(TargetList.mList.Count - 1);
      return lRes;
    }

    public IDerivation PeekBegin()
    {
      CheckIfEmpty();
      return TargetList.mList[0];
    }

    public IDerivation PeekEnd()
    {
      CheckIfEmpty();
      return TargetList.mList[TargetList.mList.Count - 1];
    }

    public IDerivation GetAllDeque()
    {
      List<IDerivation> lList = new List<IDerivation>(TargetList.mList);
      if (null != TargetList.ParentDerivation)
      {
        DerivationBase lParent = TargetList.ParentDerivation.ParentDerivation;
        while (null != lParent)
        {
          IDerivation d = lParent.FindDerivation(TargetListName);
          if (d != null)
          {
            //string s = d.ToString();
            ListDerivation ld = d as ListDerivation;
            if (null != ld)
            {
              lList.AddRange(ld.mList);

              lParent = ld.ParentDerivation;
              if (null != lParent)
              {
                lParent = lParent.ParentDerivation;
              }
            }
          }
          else
          {
            lParent = null;
          }
        }
      }

      int randIndex = lList.Count + 1;
      NormalDistribution nd = new NormalDistribution(0, 1000);
      while (randIndex >= lList.Count)
      {
        randIndex = Math.Abs(nd.intND());
      }
      return lList[randIndex];
    }

    private void CheckIfEmpty()
    {
      if (TargetList.mList.Count == 0)
      {
        StackTrace s = new StackTrace();
        string lCallerMethod = s.GetFrame(1).GetMethod().Name;
        throw new GrammarTransductorException(string.Format("try {0} from empty list", lCallerMethod));
      }
    }
  }
}