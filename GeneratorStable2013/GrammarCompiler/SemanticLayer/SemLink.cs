 
using System;

namespace GrammarCompiler.SemanticLayer
{
  [Serializable]
  public class SemLink
  {
    public string Declaration;
    public string Usage;
    public LinkDirection Direction;
  }
}