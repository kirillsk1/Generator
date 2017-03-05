using System;
using System.Collections.Generic;
using System.Diagnostics;
using GrammarCompiler.PhraseHierarchy;
using GraphTools;

namespace GrammarCompiler.GrammarVisitors
{
  [Flags]
  public enum GrammarGraphEdgeAttrs
  {
    None = 0,
    IsCyclic = 1,
    IsSequence = 2,
    IsPunktir = 4
  }
  [Flags]
  public enum GrammarGraphNodeAttrs
  {
    None = 0,
    IsCyclic = 1,
    IsTerminal = 2
  }

  internal class BuildGraphContext : DerivationContext
  {
    public tGraph BuildingGraph;
    public string GeneratedToName;
    //public IPhrase Parent;

    public BuildGraphContext(Grammar aGrammar, tGraph aGraph)
      : base(aGrammar)
    {
      if (aGraph == null) throw new ArgumentNullException();
      BuildingGraph = aGraph;
    }

    public BuildGraphContext(BuildGraphContext aBuildGraphContext)
      : base(aBuildGraphContext.Grammar)
    {
      BuildingGraph = aBuildGraphContext.BuildingGraph;
    }
  }

  /// <summary>
  /// —троит граф граматики
  /// </summary>
  public class BuildGraphVisitor : WalkVisitorBase
  {
    public bool OmmitTerminals = false;

    private tEdge addEdge(DerivationContext aContext, IPhrase aPhr, IPhrase aParent)
    {
      tGraph graph = (aContext as BuildGraphContext).BuildingGraph;      
      string fromName = "unk";
      string toName = "unk";
      // determine fromName
      if (aParent == null)
      {
        fromName = aContext.Grammar.MainSymbol.CounterName;
        graph.ensureExistsNode(fromName, aContext.Grammar.MainSymbol.Text);
      }
      else
      {
        NonTerminal parentNt = aParent as NonTerminal;
        if (parentNt != null)
        {
          fromName = parentNt.CounterName;
          graph.ensureExistsNode(parentNt.CounterName, parentNt.Text);
        }
        else
        {
          fromName = (aContext as BuildGraphContext).GeneratedToName;
          Debug.Assert(!string.IsNullOrEmpty(fromName), "Ќе должно быть");
        }
      }
      // determine toName
      NonTerminal nonTerminal = aPhr as NonTerminal;
      Terminal terminal = aPhr as Terminal;
      if (nonTerminal != null)
      {
        toName = nonTerminal.CounterName;
        string text = nonTerminal.Text;
        if ((nonTerminal.CycicKind & CycicKind.SkippedAsSeen) > 0) text += " *";
        graph.ensureExistsNode(toName, text);
      }
      else if (terminal != null)
      {
        string text = string.IsNullOrEmpty(terminal.Text) ? "e" : terminal.Text;
        tNode n =graph.addNonUniqueNode(text);
        toName = n.Name;
        n.CustomAttributes = "term";
      }
      else
      {        
        toName = graph.addNonUniqueNode(aPhr.ToString()).Name;
        //надо запомнить это в контексте
        (aContext as BuildGraphContext).GeneratedToName = toName;
      }
      tEdge e = graph.addEdge(fromName, toName);
      if (e == null)
        Debug.WriteLine("не создалась дуга, потому что нет вершин");
      return e;
    }

    public override IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      for (int i = 0; i < aAlternativeSet.Count; i++)
      {
        IPhrase phr = aAlternativeSet.Phrases[i];
        if (OmmitTerminals && phr is Terminal) continue;
        tEdge edge = addEdge(aContext, phr, aAlternativeSet.Parent);
        edge.CustomAttributes = phr.IsCyclic ? GrammarGraphEdgeAttrs.IsCyclic : GrammarGraphEdgeAttrs.None;

        phr.Accept(NewContext(aAlternativeSet, aContext));
      }
      return null;
    }

    public override IDerivation Visit(Seqence aSeqence, DerivationContext aContext)
    {
      for (int i = 0; i < aSeqence.Count; i++)
      {
        IPhrase phr = aSeqence.Phrases[i];
        if (OmmitTerminals && phr is Terminal) continue;        
        tEdge edge = addEdge(aContext, phr, aSeqence.Parent);
        edge.CustomAttributes = GrammarGraphEdgeAttrs.IsSequence 
          | (phr.IsCyclic ? GrammarGraphEdgeAttrs.IsCyclic : GrammarGraphEdgeAttrs.None);

        phr.Accept(NewContext(aSeqence, aContext));
      }
      return null;
    }

    public override IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      tGraph graph = (aContext as BuildGraphContext).BuildingGraph;            
      if ((aSymbol.CycicKind & CycicKind.CyclicOrigin) > 0)
      {
        graph.ensureExistsNode(aSymbol.CounterName, aSymbol.Text);
        graph.ensureExistsNode(aSymbol.CyclicToOccurence, aSymbol.Text);
        tEdge e = graph.addEdge(aSymbol.CounterName, aSymbol.CyclicToOccurence);
        if (e != null)
        {
          e.CustomAttributes = GrammarGraphEdgeAttrs.IsPunktir;
        }
      }
      if (aSymbol.CycicKind == CycicKind.None
        || aSymbol.CycicKind == CycicKind.CyclicPropagated)
      {
        aSymbol.FindItsRule().Expand(aContext);
      }
      return null;
    }
  }
}