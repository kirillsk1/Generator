 
using System.Collections.Generic;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public class ProtocolSequence : ProtocolBase
  {
    //private Dictionary<string, ProtocolSequence> mDic;
    private readonly Dictionary<string, List<NonTerminal>> mDic;

    public ProtocolSequence()
    {
      //mDic = new Dictionary<string, ProtocolSequence>();
      mDic = new Dictionary<string, List<NonTerminal>>();
    }

    public void Add(string aFieldName, List<NonTerminal> aRes)
    {
      mDic.Add(aFieldName, aRes);
    }

    public List<NonTerminal> ToList()
    {
      List<NonTerminal> lRes = new List<NonTerminal>();
      foreach (List<NonTerminal> list in mDic.Values)
      {
        lRes.AddRange(list);
      }
      return lRes;
    }


    public void Clear()
    {
      mDic.Clear();
    }
  }
}