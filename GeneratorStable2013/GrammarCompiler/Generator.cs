 
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;
using GrammarCompiler.ResultSaver;
using XmlParsing;
using System.Threading;

namespace GrammarCompiler
{
  /// <summary>
  /// Отвечает за вывод цепочек
  /// </summary>
  public class Generator
  {
    private readonly Grammar mGrammar;

    public Grammar Grammar
    {
      get { return mGrammar; }
    }

    public TreeNode DerivTree;
    public string ConfigFilePath;
    public static XmlParser ConfigXml;
    public static CounterDictionary CounterDictionary;

    public GrammarOptions Options = new GrammarOptions();
    public static VisitorExpandBase VisitorExpandRnd = VisitorExpandFactory.CreateVisitor(AlternativeSelectAlg.RndDistr);

    public static VisitorExpandBase VisitorExpandNormRnd =
      VisitorExpandFactory.CreateVisitor(AlternativeSelectAlg.NormalDistr);

    public static VisitorExpandBase VisitorExpandForceEnum =
      VisitorExpandFactory.CreateVisitor(AlternativeSelectAlg.Enum);

    public eGenerationMode Mode = eGenerationMode.IterativeTopDown;
    public IResultSaver ResultSaver = new ConcatResultSaver();
    public bool StopGenerate;
    public bool PauseGenerate;
    public bool IsEnumFinished; //Перебор завершен
    private int MaxListDerLevel;

    /// <summary>
    /// Происходит, когда завершен один условный шаг вывода.
    /// При обходе IterativeTopDown это раскрытие одного из списков
    /// При обходе IterativeLeftRight это раскрытие слоя
    /// Выдает промежуточные результаты
    /// </summary>
    public event EventHandler<GenerateProgressEventArgs> Progress;

    private void Reset()
    {
      ResultSaver.Reset();
      //сбросить счетчики
      CounterDictionary = new CounterDictionary();
      IsEnumFinished = false;
    }

    /// <summary>
    /// Самый главный метод генерирует все программы
    /// (столько сколько задано в опциях или сколько получится при переборе)
    /// </summary>
    /// <returns>Текст программы или лог перебора</returns>
    public string GenerateAll()
    {
      if (mGrammar.SyntaxErrors.Count > 0)
      {
        return "please, correct syntax errors first";
      }
      if (mGrammar.MainSymbol == null)
      {
        return "no main symbol";
      }

      if (Options.AlternativeAlg == AlternativeSelectAlg.Enum && Options.AllEnumInOne)
      {
        return GenerateEnum();
      }
      if (Options.AlternativeAlg == AlternativeSelectAlg.Pairs)
      {
        return GeneratePairs();
      }

      Reset();
      //while (ResultSaver.SavedCount < Options.TestsAmount 
     //   && 
      //  ResultSaver.BadResults < 10
     //  )
        //&& !IsEnumFinished)
      //while (ResultSaver.BadResults < 10)
      {
        CreateContext();
        GenerateSingleText();
      }
      return ResultSaver.GetFinalLog();
    }

    private string GeneratePairs()
    {
      PairEnumerator lPairs = new PairEnumerator(this);
      //чтобы посмотреть, какие пары достижимы TODO переделать для рисования таблицы      
      //lPairs.Enumerate(null);
      lPairs.Enumerate(delegate(string aRuleA, string aRuleB, List<string> aPath)
                         {
                           //в специальном визиторе запоминаем опорный путь - прутик
                           CreateContext();
                           VisitorExpandPairs lVisitor = mGrammar.MainSymbol.Context.Visitor as VisitorExpandPairs;
                           aPath.RemoveAt(0);
                           lVisitor.Twig = new Twig(aPath);

                           GenerateSingleText();
                         });
      return ResultSaver.GetFinalLog();
    }


    private string GenerateEnum()
    {

      while (!IsEnumFinished)
      {
        CreateContext();
        GenerateSingleText();
        //moved to the end of GenerateSingleText() //CounterDictionary.Increase();
      }
      return ResultSaver.GetFinalLog();
    }

    private void CreateContext()
    {
      DerivTree = new TreeNode("Root");
      DerivationContext lContext = new DerivationContext(mGrammar);
      lContext.Generator = this;
      lContext.Visitor = VisitorExpandFactory.CreateVisitor(Options.AlternativeAlg);
      lContext.RuleDerivNode = DerivTree;
      mGrammar.MainSymbol.Context = lContext;
    }

    /// <summary>
    /// Генерирует один текст
    /// </summary>
    /// <returns></returns>
    private string GenerateSingleText()
    {
      //Clear
      //  RuleProtocol = new DictionaryDerivation(true, new DerivationContext(this));
      mGrammar.SysTrans.Clear();
      StopGenerate = false;

      mGrammar.AllNonTerminals.Clear();
      VisitorExpandBase.PlaceHolders.Clear();

      if (File.Exists(ConfigFilePath))
      {
        ConfigXml = new XmlParser(ConfigFilePath);
      }

      IDerivation lDerivation = null;
      switch (Mode)
      {
        case eGenerationMode.RecursiveTopDown:
          lDerivation = ExpandNonTerminal(mGrammar.MainSymbol);
          break;
        case eGenerationMode.IterativeTopDown:
          lDerivation = GenerateIterativeTopDown();
          break;
        case eGenerationMode.IterativeLeftRight:
          lDerivation = GenerateIterativeLeftRight(mGrammar.MainSymbol.Context);
          break;
      }
      if (Grammar.SpecGraphBuilder != null) Grammar.SpecGraphBuilder.RootObject = lDerivation;
      //convert generated tree to text
      string lGeneratedText = lDerivation.ToString();
      //save it
      ResultSaver.Save(lGeneratedText);
      mGrammar.MainSymbol.Context.DerivationXml.Save("c://derivationXml.xml");

      IsEnumFinished = CounterDictionary.Increase();
      StopGenerate = false;

      return lGeneratedText;
    }

    private IDerivation GenerateIterativeTopDown()
    {
      ListDerivation lRootList = new ListDerivation(true, mGrammar.MainSymbol.Context);
      lRootList.Add(ExpandNonTerminal(mGrammar.MainSymbol));
      ListDerivation lListDer = null;
      ListDerivation lNextListDer = lRootList;
      MaxListDerLevel = 0;

      ListDerivation fixedLevel = null;
      while (null != lNextListDer)
      {        
        lListDer = lNextListDer;
        lNextListDer = lListDer.ExpandStep();

        // fire progress event
        if (Progress != null && lNextListDer != null)
        {
          if (lNextListDer.Level == 2)
          {
            fixedLevel = lNextListDer;
            //MessageBox.Show(fixedLevel.ToString());
          }        
          if (lNextListDer.Level > MaxListDerLevel)
          {
            MaxListDerLevel = lNextListDer.Level;
          }
          //GenerateProgressEventArgs e = new GenerateProgressEventArgs
          //                                {
          //                                  RootText = lRootList.ToString(),
          //                                  CurrentListText = lNextListDer.ToString(),
          //                                  CurrentListLevel = lNextListDer.Level,
          //                                  MaxLevel = MaxListDerLevel,
          //                                  CurrentInCurrent = lNextListDer.mCurrentItemIndex
          //                                };
          //if (fixedLevel != null)
          //{
          //  e.CurrentInRoot = fixedLevel.mCurrentItemIndex;
          //  e.TotalInRoot = fixedLevel.mList.Count;
          //}
          //if (PauseGenerate)
          //{
          //  e.PausedAtList = lNextListDer;
          //}
          //Progress(this, e);
          while (PauseGenerate)
          {
            Thread.Sleep(100);
          }
        }
      }
      return lListDer;
    }


    public static IDerivation ExpandNonTerminal(NonTerminal aSymbol)
    {
        //взали правило
      Rule lRule = aSymbol.FindItsRule();

      aSymbol.Context.ExpandingRuleSymbol = aSymbol;

      //Раскрываем правую часть правила
      IDerivation lRuleResult = lRule.Expand(aSymbol.Context);
      //!!! aSymbol.mGrammar.RuleProtocol.Add(aSymbol.Text, lRuleResult, aSymbol.Context);
      return lRuleResult; // Expand(lRuleResult, aContext);      
    }

    private ListDerivation GenerateIterativeLeftRight(DerivationContext aLContext)
    {
      ListDerivation lCurDerivationList = new ListDerivation(true, aLContext);
      //начало - генерируем первый вывод в цепочке вывода
      lCurDerivationList.Add(mGrammar.MainSymbol.Accept(aLContext));
      bool lDeriving = true;
      while (lDeriving)
      {
        lDeriving = false;

        //log
        //TLog.Write("===>" + lCurDerivationList);
        // fire progress event
        if (Progress != null && lCurDerivationList != null)
        {
          GenerateProgressEventArgs e = new GenerateProgressEventArgs
          {
            RootText = lCurDerivationList.ToString(),
          };
          Progress(this, e);
        }

        //создаем новый список и переписываем в него lCurDerivationList раскрывая нетерминалы
        ListDerivation lNewDerivationList = new ListDerivation(true, aLContext);
        //Просматриваем список на предмет нетерминалов и раскрываем их
        for (int i = 0; i < lCurDerivationList.mList.Count; i++)
        {
          SymbolDerivation lNonTermSym = lCurDerivationList[i] as SymbolDerivation;
          if (null != lNonTermSym)
          {
//раскрываем нетерминал
            lDeriving = true;
            IDerivation lDer = ExpandNonTerminal(lNonTermSym.Symbol);
            //вставить результат в новый список
            if (lDer is ListDerivation)
            {
              ListDerivation list = (ListDerivation) lDer;
              foreach (IDerivation ddd in list.mList)
              {
                lNewDerivationList.Add(ddd);
              }
            }
            else
            {
              lNewDerivationList.Add(lDer);
            }
          }
          else
          {
            //скопировать терминал в новый список
            lNewDerivationList.Add(lCurDerivationList[i]);
          }

          //SpecGraphBuilder.RootObject = lNewDerivationList;
          //Thread.Sleep(500);
        }
        lCurDerivationList = lNewDerivationList;
      }

      if (Grammar.SpecGraphBuilder != null)
      Grammar.SpecGraphBuilder.RootObject = lCurDerivationList;

      return lCurDerivationList;
    }

    public Generator(Grammar aGrammar)
    {
      if (aGrammar == null) throw new ArgumentNullException();
      mGrammar = aGrammar;

      CounterDictionary = new CounterDictionary();
    }
  }
}