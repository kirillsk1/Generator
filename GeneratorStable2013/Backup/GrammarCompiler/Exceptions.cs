 
using System;

namespace GrammarCompiler
{
  public class GrammarSyntaxException : ApplicationException
  {
    public int Col = -1;
    public int Line = -1;
    public string LineText;

    public GrammarSyntaxException(string aMessage)
      : base(aMessage)
    {
    }

    public GrammarSyntaxException(string aMessage, string aLineText) : base(aMessage)
    {
      LineText = aLineText;
    }
  }

  public class LexerInternalException : GrammarSyntaxException
  {
    public int I;
    public int Q;

    public LexerInternalException(string aMessage, string aLineText, int q, int i)
      : base(aMessage, aLineText)
    {
      Q = q;
      I = i;
    }
  }

  public class GrammarDeductException : ApplicationException
  {
    public Rule Rule = null;

    public GrammarDeductException(string aMessage)
      : base(aMessage)
    {
    }
  }

  public class GrammarCounterEndException : ApplicationException
  {
    public GrammarCounterEndException()
      : base("Enumerating deduction is finished!")
    {
    }
  }

  public class GrammarTransductorException : ApplicationException
  {
    public GrammarTransductorException(string aMessage)
      : base(aMessage)
    {
    }
  }
}