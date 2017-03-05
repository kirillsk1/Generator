 
using System.Collections.Generic;
using System.Diagnostics;
using GrammarCompiler.PhraseHierarchy;
using GraphTools;

namespace GrammarCompiler.GrammarVisitors
{
  internal class CyclesDetectContext : DerivationContext //Base
  {
    public Dictionary<string, NonTerminal> NotTerminals;
    public IPhrase Parent;
    //public tGraph BuildingGraph;
    
    public CyclesDetectContext(Grammar aGrammar)
      : base(aGrammar)
    {
      NotTerminals = new Dictionary<string, NonTerminal>();
    }

    public CyclesDetectContext(CyclesDetectContext aCyclesDetectContext)
      : base(aCyclesDetectContext.Grammar)
    {
      NotTerminals = new Dictionary<string, NonTerminal>(aCyclesDetectContext.NotTerminals);
      Visitor = aCyclesDetectContext.Visitor;
      //BuildingGraph = aCyclesDetectContext.BuildingGraph;
    }
  }

  /// <summary>
  /// Выполняется подстановка всего, что можно.
  /// </summary>    
  internal class CyclesDetectVisitor : WalkVisitorBase
  {
    // Каждый раз при раскрытии нетерминала будем добавлять его в этот список с тем, чтобы
    // никогда не раскрывать один и тот же символ дважды.
    private readonly List<string> mAlreadyUsedSymbols = new List<string>();

    public override IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aSymbol.Parent = context.Parent;
      if (context.NotTerminals.ContainsKey(aSymbol.Text))
      {
        aSymbol.CycicKind = CycicKind.CyclicOrigin;
        aSymbol.CyclicToOccurence = context.NotTerminals[aSymbol.Text].CounterName;
        aSymbol.PropagateCycle();        
        //aContext.Grammar.AllCyclicSymbolOccurs.Add(aSymbol);
      }
      else
      {
        bool isAlreadyUsed = mAlreadyUsedSymbols.Contains(aSymbol.Text);
        mAlreadyUsedSymbols.Add(aSymbol.Text);

        if (!isAlreadyUsed)
        {
          // Add current symbol to derivation path so that we can track cycles
          (aContext as CyclesDetectContext).NotTerminals.Add(aSymbol.Text, aSymbol);
            //взали правило
          Rule lRule = aSymbol.FindItsRule();

          //Раскрываем правую часть правила
          CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
          newContext.Parent = aSymbol;
          lRule.Expand(newContext);
        }
        else
        {
          aSymbol.CycicKind |= CycicKind.SkippedAsSeen;
        }
      }
      return null;
    }

    #region Эти переопределения нужны для создания нового контекста и связей Parent

    protected override DerivationContext NewContext(IPhrase aPhrase, DerivationContext aContext)
    {
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aPhrase;
      return newContext;
    }

    public override IDerivation Visit(Seqence aSeqence, DerivationContext aContext)
    {
      aSeqence.Parent = (aContext as CyclesDetectContext).Parent;
      return base.Visit(aSeqence, aContext);
    }

    public override IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      aAlternativeSet.Parent = (aContext as CyclesDetectContext).Parent;
      return base.Visit(aAlternativeSet, aContext);
    }

    public override IDerivation Visit(Terminal aSymbol, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aSymbol.Parent = context.Parent;
      return null;
    }

    public override IDerivation Visit(Access aAccess, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aAccess.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aAccess;
      return base.Visit(aAccess, newContext);
    }

    public override IDerivation Visit(AccessSeq aAccessSeq, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aAccessSeq.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aAccessSeq;
      return base.Visit(aAccessSeq, newContext);
    }

    public override IDerivation Visit(AccessArray aAccessArray, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aAccessArray.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aAccessArray;
      return base.Visit(aAccessArray, newContext);
    }

    public override IDerivation Visit(ExprDouble aExpr, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aExpr.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aExpr;
      return base.Visit(aExpr, newContext);
    }

    public override IDerivation Visit(ExprInt aExpr, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aExpr.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aExpr;
      return base.Visit(aExpr, newContext);
    }

    public override IDerivation Visit(ExprOp aExprOp, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aExprOp.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aExprOp;
      return base.Visit(aExprOp, newContext);
    }

    public override IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aQuantifiedPhrase.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aQuantifiedPhrase;
      return base.Visit(aQuantifiedPhrase, newContext);
    }

    public override IDerivation Visit(TransCallPhrase aTransCallPhrase, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aTransCallPhrase.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aTransCallPhrase;
      return base.Visit(aTransCallPhrase, newContext);
    }

    public override IDerivation Visit(PlaceHolderAssignPhrase aPlaceHolderAssignPhrase, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aPlaceHolderAssignPhrase.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aPlaceHolderAssignPhrase;
      return base.Visit(aPlaceHolderAssignPhrase, newContext);
    }

    public override IDerivation Visit(PlaceHolderPhrase aPlaceHolderPhrase, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aPlaceHolderPhrase.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aPlaceHolderPhrase;
      return base.Visit(aPlaceHolderPhrase, newContext);
    }

    public override IDerivation Visit(IPhrase aPhr, DerivationContext aContext)
    {
      CyclesDetectContext context = aContext as CyclesDetectContext;
      aPhr.Parent = context.Parent;
      CyclesDetectContext newContext = new CyclesDetectContext(aContext as CyclesDetectContext);
      newContext.Parent = aPhr;
      return base.Visit(aPhr, newContext);
    }

    #endregion
  }
}