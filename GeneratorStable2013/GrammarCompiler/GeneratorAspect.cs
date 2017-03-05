/**********************************************************
 *            BNF Test Generator                          *
 * Описание:                                              *
 *     Аспект генерации - содержит методы раскрытия       *
 *     нетерминальных символов для последовательности,    *
 *     альтернативы и т.д.                                *
 *                                                        *
 * Дата: 25.12.2007                                       *
 *                                                        *
 **********************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Tools;

namespace GrammarCompiler
{
  public partial class AlternativeSet
  {
    private static readonly Random mRnd = new Random();

    protected override IDerivation ExpandForce(DerivationContext aContext)
    {
      //1. Взять контекстный счетчик
      //1.1 Найти в словаре, если нет, создать
      Counter lCounter = Grammar.CounterDictionary.FindOrAddCounter(aContext.ExpandingRuleSymbol.CounterName, mPhrases.Count);    
      //2. увеличить
      lCounter.Increase();
      return GetAlternative(aContext, lCounter.Value);
    }

    protected override IDerivation ExpandRnd(DerivationContext aContext)
    {
      int i = 0;
      //читаем из xml
      List<string> lXmlPathList = aContext.DerivPath;
      string[] lXmlPath = lXmlPathList.ToArray();
      string lRR = Grammar.mConfigXml.Search(lXmlPath, "rr");          
      if (!string.IsNullOrEmpty(lRR))
      {
        i = RndByRR(lRR);
      }
      else
      {
        i = mRnd.Next(mPhrases.Count);
      }
      return GetAlternative(aContext,i);
    }

    public IDerivation GetAlternative(DerivationContext aContext, int i)
    {
      return mPhrases[i].Expand(aContext);
    }

    private int RndByRR(string aRR)
    {
      string[] lRRNumbs = aRR.Split(' ');
      int[] RR = new int[mPhrases.Count];
      int Sum = 0;
      int i;
      //Конвертируем в int, суммируем и сохраняем функцию распределения F(x)
      for (i = 0; i < mPhrases.Count; i++)
      {
        int k = 0;
        if (i < lRRNumbs.Length)
        {
          k = int.Parse(lRRNumbs[i]);
        }
        Sum += k;
        RR[i] = Sum;
      }
      int n = mRnd.Next(Sum);
      i = 0;
      while (n > RR[i])
      {
        i++;
      }
      return i;
    }
  }

  partial class Seqence
  {
    protected override IDerivation ExpandRnd(DerivationContext aContext)
    {
      DictionaryDerivation lList = new DictionaryDerivation(InsertSpace, aContext);
      int lExprCnt = 1;
      for (int i = 0; i < mPhrases.Count; i++)
      {
        IPhrase lPhr = mPhrases[i];
        IDerivation lRes = lPhr.Expand(aContext);
        string lKeyName;
        Symbol lCurrentSymbol = lPhr as Symbol;
        if (lCurrentSymbol != null)
        {
          lCurrentSymbol.OccurenceInSeq = i;
          lKeyName = lCurrentSymbol.Text;
          //взять протокол вывода этого правила
          //TODO: aContext.RuleSeqProtocol.Add(lCurrentSymbol.Text, lRes);
          //добавить в его протокол вывода последовательности имя lCurrentSymbol.Text и результат lRes
        }
        else
        {
          lKeyName = string.Format("Expr{0}", lExprCnt++);
        }
        lList.Add(lKeyName, lRes, aContext);
        
      }
      return lList;
    }
  }

  partial class QuantifiedPhrase
  {
    private static readonly Random mRnd = new Random();

    protected override IDerivation ExpandRnd(DerivationContext aContext)
    {
        if (IsConfigurable)
        {
            //читаем из xml
            List<string> lXmlPathList = aContext.DerivPath;
            
            Symbol lInnerSym = Phrase as Symbol;
            if (lInnerSym != null)
            {
              lXmlPathList.Add(lInnerSym.Text);
            }
        
            string[] lXmlPath = lXmlPathList.ToArray();

            int lMin = Grammar.mConfigXml.SearchInt(lXmlPath, "min");
            int lMax = Grammar.mConfigXml.SearchInt(lXmlPath, "max");
            int lCount = Grammar.mConfigXml.SearchInt(lXmlPath, "cnt");
            if (lCount != -1)
            {
                return ExpandInternal(lCount, lCount, aContext);
            }
            else
            {
                if (lMin == -1)
                {
                    lMin = (int)Min;
                }
                if (lMax == -1)
                {
                    lMax = (int)Max;
                }
                return ExpandInternal(lMin, lMax, aContext);
            }
        }
        else
        {
            return ExpandInternal((int)Min.Value, (int)Max.Value, aContext);
        }
    }

    private IDerivation ExpandInternal(int aMin, int aMax, DerivationContext aContext)
    {
      ListDerivation lList = new ListDerivation(true, aContext);
      
      int rest = 0;
      if (aMax != int.MaxValue)
      {
        rest = mRnd.Next(aMax - aMin + 1);
      }
      for (int i = 0; i < aMin + rest; i++)
      {
        lList.Add(Phrase.Expand(aContext));
      }
      //есил 5..* генерим, пока не выпадет орел :)
      if (aMax == int.MaxValue)
      {
        while (mRnd.Next(2) >= 1)
        {
          lList.Add(Phrase.Expand(aContext));
        }
      }
      return lList;
    }
  }

  partial class Symbol
  {
    protected override IDerivation ExpandRnd(DerivationContext aContext)
    {
      if (IsTerminal)
      {        
        TLog.Write("T", Text);
        return new TextDerivation(Text);
      }
      else
      {
        // искать правило
        if (mGrammar.mRules.ContainsKey(Text))
        {
          Rule lRule = mGrammar.mRules[Text];
          TreeNode lNewNode = new TreeNode(Text);
          aContext.RuleDerivNode.Nodes.Add(lNewNode);
          DerivationContext lNewContext = new DerivationContext(aContext);
          lNewContext.RuleDerivNode = lNewNode;
          lNewContext.ExpandingRuleSymbol = this;
          
          IDerivation lRuleResult = lRule.Expand(lNewContext);
          mGrammar.RuleProtocol.Add(Text, lRuleResult, aContext);
          return lRuleResult; // Expand(lRuleResult, aContext);
        }
        else
        {
          // нашли нетерм. символ, который нельзя раскрыть
          throw new GrammarDeductException(string.Format("Не определен нетерминальный символ {0}", Text));
        }
      }
    }
  }

  partial class TransCallPhrase
  {
    /// <summary>
    /// Do actual call of transductor method with parameter substitution
    /// </summary>
    /// <param name="aContext">Deriving context</param>
    /// <returns>List of symbols produced by transductor</returns>
    protected override IDerivation ExpandRnd(DerivationContext aContext)
    {
      //1. Substitute and prepare parameters form context by conf xml
      object[] parametrs = ParamsArray(aContext);

      if (mTargetAccess != null)
      {
        IDerivation lDeriv = mTargetAccess.Expand(aContext);
        mGrammar.ListTrans.TargetList = (ListDerivation)lDeriv;
      }

      mGrammar.SysTrans.Context = aContext;
      string lStrResult;// = "no trans result!!!";
      //2. do actual call
      try
      {
        object lRetVal = mBindedMethod.Invoke(mTransductorClass, parametrs);
        if (lRetVal is int)
        {
          return new ExprInt((int)lRetVal);
        }
        lStrResult = Convert.ToString(lRetVal);
      }
      catch (Exception ex)
      {
        while (ex.InnerException != null)
        {
          ex = ex.InnerException;
        }
        lStrResult = ex.Message;
      }
      TLog.Write("T", lStrResult);        
      return new TextDerivation(lStrResult);
    }

    private object[] ParamsArray(DerivationContext aContext)
    {
      int lParamsCount = mParams.Count;
      object[] parametrs = new object[lParamsCount];
      int i = 0;
      foreach (IExpr lParam in mParams)
      {
        parametrs[i] = lParam.GetValue(aContext);
        i++;
      }
      return parametrs;
    }
  }
}