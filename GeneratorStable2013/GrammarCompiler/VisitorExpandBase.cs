 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;
using GrammarCompiler.PhraseHierarchy;
using Tools;

namespace GrammarCompiler.GrammarVisitors
{
  public interface IVisitor
  {
    IDerivation Visit(Terminal aSymbol, DerivationContext aContext);
    IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext);
    IDerivation Visit(Seqence aSeqence, DerivationContext aContext);
    IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext);
    IDerivation Visit(TransCallPhrase aTransCallPhrase, DerivationContext aContext);
    IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext);
    IDerivation Visit(PlaceHolderPhrase aPlaceHolderPhrase, DerivationContext aContext);
    IDerivation Visit(PlaceHolderAssignPhrase aPlaceHolderAssignPhrase, DerivationContext aContext);
    IDerivation Visit(ExprOp aExprOp, DerivationContext aContext);
    IDerivation Visit(Access aAccess, DerivationContext aContext);
    IDerivation Visit(AccessArray aAccessArray, DerivationContext aContext);
    IDerivation Visit(AccessSeq aAccessSeq, DerivationContext aContext);
    IDerivation Visit(ExprInt aExpr, DerivationContext aContext);
    IDerivation Visit(ExprDouble aExpr, DerivationContext aContext);
    IDerivation Visit(IPhrase aPhr, DerivationContext aContext);
  }

  public abstract class VisitorExpandBase : IVisitor
  {
    protected static readonly Random mRnd = new Random();

    public virtual IDerivation Visit(Terminal aSymbol, DerivationContext aContext)
    {
      TLog.Write("T", aSymbol.Text);

      XmlNode newNode = aContext.DerivationXml.CreateElement("term");
      aContext.currentNode.AppendChild(newNode);
      newNode.InnerText = aSymbol.Text;
      //aContext.DerivationXml.CreateTextNode(aSymbol.Text);

      return new TextDerivation(aSymbol.Text);
    }

    public virtual IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      //дл€ дерева вывода и контекста
      TreeNode lNewNode = new TreeNode(aSymbol.Text);
      aContext.RuleDerivNode.Nodes.Add(lNewNode);
      aSymbol.Context = new DerivationContext(aContext);
      aSymbol.Context.RuleDerivNode = lNewNode;
      XmlNode newNode = aContext.DerivationXml.CreateElement(aSymbol.Text);
      aContext.currentNode.AppendChild(newNode);
      aSymbol.Context.currentNode = newNode;

      //ќграничиваем уровень вложенности!!!!!!!!!!!!!!!!
      if (aContext.LevelCount > aContext.Generator.Options.LevelRestriction && !aContext.Generator.StopGenerate)
      {
        aContext.Generator.StopGenerate = true;
        //MessageBox.Show("ќстанавливаемс€...");
      }

      switch (aContext.Generator.Mode)
      {
        case eGenerationMode.RecursiveTopDown:
          return Generator.ExpandNonTerminal(aSymbol);
        case eGenerationMode.IterativeTopDown:
        case eGenerationMode.IterativeLeftRight:
          return new SymbolDerivation(aSymbol);
      }
      //return null;//never
      throw new GrammarDeductException("should never been happen");
    }

    public virtual IDerivation Visit(Seqence aSeqence, DerivationContext aContext)
    {
      DictionaryDerivation lList = new DictionaryDerivation(aSeqence.InsertSpace, aContext);
      int lExprCnt = 1;
      for (int i = 0; i < aSeqence.Count; i++)
      {
        IPhrase lPhr = aSeqence.Phrases[i];
        IDerivation lRes = lPhr.Accept(aContext);
        string lKeyName;
        NonTerminal lCurrentSymbol = lPhr as NonTerminal;
        QuantifiedPhrase lCurQuantPhr = lPhr as QuantifiedPhrase;
        if (null != lCurQuantPhr)
        {
          lCurrentSymbol = lCurQuantPhr.Phrase as NonTerminal;
        }
        if (lCurrentSymbol != null)
        {
          //to del lCurrentSymbol.OccurenceInSeq = i;
          lKeyName = lCurrentSymbol.Text;
          //вз€ть протокол вывода этого правила
          //TODO: aContext.RuleSeqProtocol.Add(lCurrentSymbol.Text, lRes);
          //добавить в его протокол вывода последовательности им€ lCurrentSymbol.Text и результат lRes
        }
        else
        {
          lKeyName = string.Format("Expr{0}", lExprCnt++);
        }
        if (!string.IsNullOrEmpty(lPhr.Alias))
        {
          lKeyName = lPhr.Alias;
        }
        lList.Add(lKeyName, lRes, aContext);
      }
      return lList;
    }

    public virtual IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      //считать конфигурацию узла и передать управление соотв. визитору
      //читаем из xml
      if (Generator.ConfigXml != null)
      {
        List<string> lXmlPathList = aContext.DerivPath;
        string[] lXmlPath = lXmlPathList.ToArray();
        string path = string.Join("/", lXmlPath);

        //RR р€д распределени€
        int i = 0;
        string lRR = null;
        if (aContext.Generator.Options.RREnable)
        {
          lRR = Generator.ConfigXml.SearchAtRootOnly(lXmlPath[lXmlPath.Length - 1], "rr");
        }
        if (!string.IsNullOrEmpty(lRR))
        {
          i = RndByRR(aAlternativeSet, lRR);
          return aAlternativeSet.GetAlternative(aContext, i);
        }

        string lAlg = "";
        lAlg = ""; // Generator.ConfigXml.Search(lXmlPath, "alg");
        if (path == "Root/bodyStats/bodyStat")
        {
          lAlg = "enum";
        }
        if (!string.IsNullOrEmpty(lAlg))
        {
          //DerivationContext lNewCont = new DerivationContext(aContext);
          IVisitor lVisitor = aContext.Visitor;
          switch (lAlg)
          {
            case "rnd":
              lVisitor = Generator.VisitorExpandRnd;
              break;
            case "enum":
              lVisitor = Generator.VisitorExpandForceEnum;
              break;
            case "norm":
              lVisitor = Generator.VisitorExpandNormRnd;
              break;
            default:
              throw new ConfigurationException(
                "Ќеправильное значение аттрибута alg в конфигурационном файле. ƒопустимо одно из трех: rnd, enum, norm");
          }
          return (lVisitor as VisitorExpandBase).VisitInternal(aAlternativeSet, aContext);
        }
      }
      return VisitInternal(aAlternativeSet, aContext);
    }

    public abstract IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext);

    private int RndByRR(AlternativeSet aAlternativeSet, string aRR)
    {
      string[] lRRNumbs = aRR.Split(' ');
      int[] RR = new int[aAlternativeSet.Count];
      int Sum = 0;
      int i;
      // онвертируем в int, суммируем и сохран€ем функцию распределени€ F(x)
      for (i = 0; i < aAlternativeSet.Count; i++)
      {
        int k = 1; //«начение по умолчанию. ≈сли есть три альтернативы и rr="10", то подразумеваетс€, что rr="10 1 1"
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

    public virtual IDerivation Visit(TransCallPhrase aTransCallPhrase, DerivationContext aContext)
    {
      eGenerationMode lOldMode = aContext.Generator.Mode;
      aContext.Generator.Mode = eGenerationMode.RecursiveTopDown;

      //1. Substitute and prepare parameters form context by conf xml
      object[] parametrs = ParamsArray(aTransCallPhrase, aContext);

      if (aTransCallPhrase.TargetAccess != null)
      {
        IDerivation lDeriv = aTransCallPhrase.TargetAccess.Accept(aContext);
        aTransCallPhrase.Grammar.ListTrans.TargetList = (ListDerivation) lDeriv;
        aTransCallPhrase.Grammar.ListTrans.TargetListName = aTransCallPhrase.TargetAccess.ObjectName;
      }

      aTransCallPhrase.Grammar.SysTrans.Context = aContext;
      //2. do actual call
      object lRetVal = null;
      try
      {
        lRetVal = aTransCallPhrase.BindedMethod.Invoke(aTransCallPhrase.TransductorClass, parametrs);
      }
      catch (Exception ex)
      {
        while (ex.InnerException != null)
        {
          ex = ex.InnerException;
        }
        lRetVal = ex.Message;
      }
      aContext.Generator.Mode = lOldMode;
      if (lRetVal is int)
      {
        lRetVal = new ExprInt((int) lRetVal);
      }

      IDerivation lDer = lRetVal as IDerivation;
      if (null == lDer)
      {
        lDer = new TextDerivation(lRetVal.ToString());
      }
      XmlNode newNode = aContext.DerivationXml.CreateElement("trans");
      aContext.currentNode.AppendChild(newNode);
      newNode.InnerText = lDer.ToString();

      TLog.Write("T", lDer.ToString());
      return lDer;
    }

    //public abstract IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext);
    public virtual IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext)
    {
      if (aQuantifiedPhrase.IsConfigurable)
      {
        //читаем из xml
        List<string> lXmlPathList = aContext.DerivPath;

        NonTerminal lInnerSym = aQuantifiedPhrase.Phrase as NonTerminal;
        if (lInnerSym != null)
        {
          lXmlPathList.Add(lInnerSym.Text);
        }

        string[] lXmlPath = lXmlPathList.ToArray();

        int lMin = Generator.ConfigXml.SearchInt(lXmlPath, "min");
        int lMax = Generator.ConfigXml.SearchInt(lXmlPath, "max");
        int lCount = Generator.ConfigXml.SearchInt(lXmlPath, "cnt");
        if (lCount != -1)
        {
          return ExpandInternal(aQuantifiedPhrase, lCount, lCount, aContext);
        }
        if (lMin == -1)
        {
          lMin = (int) aQuantifiedPhrase.Min;
        }
        if (lMax == -1)
        {
          lMax = (int) aQuantifiedPhrase.Max;
        }
        return ExpandInternal(aQuantifiedPhrase, lMin, lMax, aContext);
      }
      return ExpandInternal(aQuantifiedPhrase, (int) aQuantifiedPhrase.Min.Value, (int) aQuantifiedPhrase.Max.Value,
                            aContext);
    }

    private IDerivation ExpandInternal(QuantifiedPhrase aQuantifiedPhrase, int aMin, int aMax,
                                       DerivationContext aContext)
    {
      // вантификатор (QuantifiedPhrase) раскрываетс€ так:
      // Phrase (это то, что под квантификатором, дублируемое выражение, например, "(a|b){2..5}" - phrase = (a|b))
      // Phrase раскрываетс€ n раз и добавл€етс€ в список ListDerivation (каждый раз результат может быть разнрый)
      //
      // {2..*} * => aMax == int.MaxValue
      // ≈сли заданы минимум и максимум, напр. a{3..5}, то определ€ем рндэшно
      // добаку к минимуму (rest), в данном случае от 0 до 2

      //ј если максимум не задан (*), то 50% выход, 50% продолжение

      ListDerivation lList = new ListDerivation(true, aContext);

      int rest = 0;
      if (aMax != int.MaxValue)
      {
        rest = mRnd.Next(aMax - aMin + 1);
      }
      int i;
      for (i = 0; i < aMin + rest; i++)
      {
        ExpandPhrase(aQuantifiedPhrase, aContext, lList, i);
      }
      //есил phrase{5..*}  генерим, пока не выпадет орел :)
      if (aMax == int.MaxValue)
      {
        while (mRnd.Next(2) >= 1)
        {
          ExpandPhrase(aQuantifiedPhrase, aContext, lList, i);
          i++;
        }
      }
      return lList;
    }

    private static void ExpandPhrase(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext,
                                     ListDerivation aList, int aRepeatCount)
    {
      if (aQuantifiedPhrase.Phrase is NonTerminal)
      {
        NonTerminal lThisSym = (NonTerminal) aQuantifiedPhrase.Phrase;
        NonTerminal lNewSymbol = new NonTerminal(lThisSym);
        //to del but i feel it will be neeeded lNewSymbol.OccurenceInSeq = aRepeatCount;
        aList.Add(lNewSymbol.Accept(aContext));
      }
      else
      {
        aList.Add(aQuantifiedPhrase.Phrase.Accept(aContext));
      }
    }

    public virtual IDerivation Visit(PlaceHolderPhrase aPlaceHolderPhrase, DerivationContext aContext)
    {
      ListDerivation lList = new ListDerivation(true, aContext);
      TextDerivation lText = new TextDerivation(aPlaceHolderPhrase.Name); //Symbol.Create(mGrammar, Name, true);
      lList.Add(lText);
      //add this point to special list for futher replacement
      PlaceHolders.Add(aPlaceHolderPhrase.Name, lText);
      return lList;
    }

    public virtual IDerivation Visit(PlaceHolderAssignPhrase aPlaceHolderAssignPhrase, DerivationContext aContext)
    {
      IDerivation lExpandList = aPlaceHolderAssignPhrase.RightPhrase.Accept(aContext);
      List<IDerivation> lReplacePoints = PlaceHolders.GetList(aPlaceHolderAssignPhrase.Name);
      foreach (TextDerivation lPoint in lReplacePoints)
      {
        if (aPlaceHolderAssignPhrase.Add)
        {
          lPoint.Text += lExpandList.ToString();
        }
        else
        {
          lPoint.Text = lExpandList.ToString();
        }
      }
      return lExpandList;
    }

    public virtual IDerivation Visit(ExprOp aExprOp, DerivationContext aContext)
    {
      aExprOp.DetermineResultType();
      if (aExprOp.Type == typeof (double))
      {
        aExprOp.dmResultAcc = new ExprDouble();
        //¬ычислить операнды, выполнить над результатом свою операцию
        aExprOp.dmResultAcc.Value = ((ExprDouble) aExprOp.Operands[0].Accept(aContext)).Value;

        for (int i = 1; i < aExprOp.Operands.Count; i++)
        {
          double lVal = ((ExprDouble) aExprOp.Operands[i].Accept(aContext)).Value;
          aExprOp.AccumulateResultd(lVal);
        }
        return aExprOp.dmResultAcc;
      }
      else
      {
        aExprOp.imResultAcc = new ExprInt();
        //¬ычислить операнды, выполнить над результатом свою операцию
        aExprOp.imResultAcc.Value = ((ExprInt) aExprOp.Operands[0].Accept(aContext)).Value;

        for (int i = 1; i < aExprOp.Operands.Count; i++)
        {
          int lVal = ((ExprInt) aExprOp.Operands[i].Accept(aContext)).Value;
          aExprOp.AccumulateResulti(lVal);
        }
        return aExprOp.imResultAcc;
      }
    }

    public virtual IDerivation Visit(Access aAccess, DerivationContext aContext)
    {
      return aContext.ParentDerivation.FindDerivation(aAccess.ObjectName);
    }

    public virtual IDerivation Visit(AccessArray aAccessArray, DerivationContext aContext)
    {
      //TODO через строку некрасиво, надо чтобы были разные типы выражений (числовые и строковые)
      //и чтобы операци€ раскрыти€ дл€ каждого типа была своей (возвращала int а не string)
      //ј здесь надо проверить тип выражени€ и если не числовой, ругнутьс€ осмысленно, причем :)
      int lInd = int.Parse(aAccessArray.IndexExpr.Accept(aContext).ToString());
      ListDerivation lList = aAccessArray.mParentAccess.Accept(aContext) as ListDerivation;
      if (lList != null)
      {
        return lList[lInd];
      }
      else
      {
        throw new GrammarDeductException("Incorrect [] access! There is no ListDerivation!");
      }
    }

    public virtual IDerivation Visit(AccessSeq aAccessSeq, DerivationContext aContext)
    {
      IDerivation lD = aAccessSeq.mParentAccess.Accept(aContext);
      /*if (lD is ListDerivation)
      {
        lD = ((ListDerivation) lD)[0];
      }*/
      DictionaryDerivation lDic = lD as DictionaryDerivation;
      if (lDic != null)
      {
        return lDic[aAccessSeq.mFieldName];
      }
      else
      {
        throw new GrammarDeductException("Incorrect . access! There is no DictionaryDerivation!");
      }
    }

    public virtual IDerivation Visit(ExprInt aExpr, DerivationContext aContext)
    {
      return aExpr;
    }

    public virtual IDerivation Visit(ExprDouble aExpr, DerivationContext aContext)
    {
      return aExpr;
    }

    public IDerivation Visit(IPhrase aPhr, DerivationContext aContext)
    {
      throw new ApplicationException("Should never be called");
    }

    public static PlaceHolderList PlaceHolders = new PlaceHolderList();

    #region helper subroutines

    private object[] ParamsArray(TransCallPhrase aTransCallPhrase, DerivationContext aContext)
    {
      int lParamsCount = aTransCallPhrase.Params.Count;
      object[] parametrs = new object[lParamsCount];
      int i = 0;
      foreach (IExpr lParam in aTransCallPhrase.Params)
      {
        parametrs[i] = lParam.GetValue(aContext);
        i++;
      }
      return parametrs;
    }

    #endregion
  }

  public class VisitorExpandFactory
  {
    public static VisitorExpandBase CreateVisitor(AlternativeSelectAlg aAlg)
    {
      switch (aAlg)
      {
        case AlternativeSelectAlg.RndDistr:
          return new VisitorExpandRnd();
        case AlternativeSelectAlg.NormalDistr:
          return new VisitorExpandNormRnd();
        case AlternativeSelectAlg.Enum:
          return new VisitorExpandForceEnum();
        case AlternativeSelectAlg.MinRnd:
          return new VisitorExpandMinRnd();
        case AlternativeSelectAlg.Pairs:
          return new VisitorExpandPairs();
        default:
          throw new ApplicationException("Invalid Visitor options");
      }
    }
  }
}