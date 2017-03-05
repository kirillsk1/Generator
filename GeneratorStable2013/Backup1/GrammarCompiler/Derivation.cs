 
using System;
using System.Collections.Generic;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  public interface IDerivation
  {
    string ToString();
    //IDerivation FindDerivation(string aName);
  }

  public abstract class DerivationBase : IDerivation
  {
    public DerivationBase ParentDerivation;

    public virtual IDerivation FindDerivation(string aName)
    {
      if (ParentDerivation != null)
      {
        return ParentDerivation.FindDerivation(aName);
      }
      return null;
    }
  }

  public class TextDerivation : DerivationBase
  {
    public string Text { get; set; }

    public TextDerivation(string aText)
    {
      Text = aText;
    }

    public override string ToString()
    {
      return Text;
    }
  }

  public class SymbolDerivation : DerivationBase
  {
    private NonTerminal mSymbol;

    public NonTerminal Symbol
    {
      get { return mSymbol; }
      set { mSymbol = value; }
    }

    public SymbolDerivation(NonTerminal aSymbol)
    {
      mSymbol = aSymbol;
    }

    public override string ToString()
    {
      return mSymbol.Text;
    }
  }

  public class ListDerivation : DerivationBase
  {
    public string Separator = " ";
    protected readonly bool mInsertSpace;
    protected readonly Grammar mGrammar;

    public readonly List<IDerivation> mList = new List<IDerivation>();
    public int mCurrentItemIndex;
    public int Level;

    public ListDerivation(bool aInsertSpace, DerivationContext aContext)
    {
      mInsertSpace = aInsertSpace;
      mGrammar = aContext.Grammar;

      ParentDerivation = aContext.ParentDerivation;
      //errro!
      aContext.ParentDerivation = this;
      ListDerivation asListDer = ParentDerivation as ListDerivation;
      if (asListDer != null)
      {
        Level = asListDer.Level + 1;
      }

      mCurrentItemIndex = 0;
    }

    public IDerivation CurrentItem
    {
      get { return mList[mCurrentItemIndex]; }
      set { mList[mCurrentItemIndex] = value; }
    }
    public NonTerminal ExpandingSymbol { get; set; }

    public void Add(IDerivation aSym)
    {
      mList.Add(aSym);
    }

    public void AddRange(IDerivation aDeriv)
    {
      mList.AddRange(((ListDerivation) aDeriv).mList);
    }

    public IDerivation this[int idx]
    {
      get { return mList[idx]; }
    }

    public override string ToString()
    {
      string lRes = "";
      if (mList.Count > 0) lRes = mList[0].ToString();
      for (int i = 1; i < mList.Count; i++)
      {
        if (mInsertSpace) lRes += Separator;
        if (mList[i] != null)
        {
          lRes += mList[i].ToString();
        }
        else
        {
          throw new GrammarDeductException("∆опа кака€-то!");
        }
      }
      return lRes;
    }

    //ƒобавлено чтобы пофиксить юнит тесты ListTrans, AddFuncs
    //ѕока в доступах не сформулирована четка€ идеологи€ и это может быть лишним
    public override IDerivation FindDerivation(string aName)
    {
      IDerivation res = null;
      foreach (DerivationBase der in mList)
      {
        res = der.FindDerivation(aName);
        if (res != null) break;
      }
      return res;
    }

    public ListDerivation ExpandStep()
    {
      //»дем вправо по списку, если:
      //  SymbolDerivation, раскрываем и замен€ем если результат ListDerivation,
      //          возвращаем его иначе себ€
      //  ListDerivation - возвр€щаем его
      //  дошли до конца списка - возвращаем верхний список (ParentDerivation)
      while (mCurrentItemIndex < mList.Count)
      {
        ListDerivation lAsListDer = CurrentItem as ListDerivation;
        if (null != lAsListDer)
        {
          mCurrentItemIndex++;
          return lAsListDer;
        }
        SymbolDerivation lAsSymDer = CurrentItem as SymbolDerivation;
        if (null != lAsSymDer)
        {
          ExpandingSymbol = lAsSymDer.Symbol;
          //раскрываем и замен€ем
          IDerivation lSymbolResult = Generator.ExpandNonTerminal(lAsSymDer.Symbol);
          CurrentItem = lSymbolResult;
          ListDerivation lSymbolResultAsList = lSymbolResult as ListDerivation;
          //если результат ListDerivation, возвращаем его иначе себ€ (старый верхний список)
          return lSymbolResultAsList ?? this;
        }
        mCurrentItemIndex++;
      }
      //дошли до конца списка
      return ParentDerivation as ListDerivation; //???      
    }
  }

  public class DictionaryDerivation : ListDerivation
  {
    private readonly Dictionary<string, int> mKeys
      = new Dictionary<string, int>();

    public DictionaryDerivation(bool aInsertSpace, DerivationContext aContext) : base(aInsertSpace, aContext)
    {
    }

    public override IDerivation FindDerivation(string aName)
    {
      if (mKeys.ContainsKey(aName))
      {
        return mList[mKeys[aName]];
      }
      return base.FindDerivation(aName);
    }


    public void Add(string aName, IDerivation aDeriv, DerivationContext aContext)
    {
      base.Add(aDeriv);
      while (mKeys.ContainsKey(aName))
      {
        aName += "1";
      }
      mKeys.Add(aName, mList.Count - 1);
      return;
    }

    public IDerivation this[string aName]
    {
      get { return mList[mKeys[aName]]; }
    }

    public void Clear()
    {
      mKeys.Clear();
      mList.Clear();
    }

    //only for graph
    public string[] Keys
    {
      get
      {
        string[] lRet = new string[mKeys.Keys.Count];
        mKeys.Keys.CopyTo(lRet, 0);
        return lRet;
      }
    }
  }
}