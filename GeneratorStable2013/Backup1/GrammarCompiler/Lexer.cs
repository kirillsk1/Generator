 
using System;
using System.Configuration;
using System.Text;

namespace GrammarCompiler
{
  public enum LexemType
  {
    Unknown,
    Terminal, // "void main()"
    NotTerminal, // statFor
    IntNumber,
    Operation, // , |
    LeftBr, // (
    RightBr, // )
    Sign, // {} .. []
    IsSign, // :=
    AddOp, // +-
    MulOp // */
  }

  public class Lexema
  {
    public readonly string Text;
    public readonly LexemType Type;
    private readonly Lexer mLexer;

    public Lexema(string aText, LexemType aType, Lexer aLexer)
    {
      Text = aText;
      Type = aType;
      mLexer = aLexer;
    }

    public Lexema Next()
    {
      return mLexer.GetNext();
    }
  }

  /// <summary>
  /// Лексический анализатор
  /// выделяет терминальные символы - последовательности в ""
  /// </summary>
  public class Lexer
  {
    private int i;
    private readonly string mInput;
    //private readonly List<Lexema> mLexemas;
    private int q;

    public bool mDebugMode;
    private string mDebugText;

    public static bool DebugMode
    {
      get
      {
        bool lDebugMode;
        bool.TryParse(ConfigurationManager.AppSettings["LexerDebug"] ?? "false", out lDebugMode);
        return lDebugMode;
      }
      set { ConfigurationManager.AppSettings["LexerDebug"] = value.ToString(); }
    }

    public string DebugText
    {
      get
      {
        return mDebugText;
        /*
        string s = "Parsing: " + mInput + "\r\n";
        foreach (Lexema lex in mLexemas)
        {
          s += string.Format("{0}: {1}\r\n", lex.Type, lex.Text);
        }
        return s;
         */
      }
    }

    public Lexer(string aInput)
    {
      mInput = aInput;
      i = 0;
      mDebugText = "Parsing: '" + aInput + "'\r\n\r\n";
      mDebugMode = DebugMode;

      //mLexemas = new List<Lexema>();
    }

    /* public void FillList()
    {
      while (true)//(i < mInput.Length)
      {
        Lexema lex = GetNext();
        if (lex == null) break;
        mLexemas.Add(lex);
      }
    }*/

/*
    public Lexema this[int idx]
    {
      get { return mLexemas[idx]; }
    }
*/
    /*public List<Lexema> Lexemas
    {
      get
      {
        return mLexemas;
      }
    }*/
/*
    public int Count
    {
      get { return mLexemas.Count; }
    }
*/

    public int Col
    {
      get { return i; }
    }

    public Lexema GetNext()
    {
      Lexema lLex = null;
      try
      {
        lLex = GetNextInternal();
        if (mDebugMode && lLex != null)
        {
          mDebugText += string.Format("{0}: '{1}'\r\n", lLex.Type, lLex.Text);
        }
      }
      catch (Exception ex)
      {
        if (mDebugMode)
        {
          mDebugText += string.Format("\r\nException in lexer: {0}\r\n", ex.Message);
        }
        else
        {
          throw;
        }
      }
      return lLex;
    }

    private Lexema GetNextInternal()
    {
      StringBuilder sb = new StringBuilder();
      LexemType e = LexemType.Unknown;
      bool stop = false;
      bool skipChar = false;
      q = 0;
      while (!stop && i < mInput.Length)
      {
        char c = mInput[i];

        switch (q)
        {
          case 0:
            if (Char.IsLetter(c) || c == '_') // | c=='#')
            {
              q = 1;
              e = LexemType.NotTerminal;
            }
            else if (Char.IsDigit(c))
            {
              q = 2;
              e = LexemType.IntNumber;
            }
            else if (c == ':')
            {
              q = 3;
            }
            else if (c == ',' || c == '|' || c == ';')
            {
              q = 4;
              e = LexemType.Operation;
            }
            else if (c == '+' || c == '-')
            {
              q = 4;
              e = LexemType.AddOp;
            }
            else if (c == '*' || c == '%')
            {
              q = 4;
              e = LexemType.MulOp;
            }
            else if (c == '/')
            {
              q = 7;
              skipChar = true;
            }
            else if (c == '(')
            {
              q = 4;
              e = LexemType.LeftBr;
            }
            else if (c == ')')
            {
              q = 4;
              e = LexemType.RightBr;
            }
            else if (c == '"')
            {
              q = 5;
              skipChar = true;
              e = LexemType.Terminal;
            }
            else if (Char.IsPunctuation(c) || c == '<' || c == '>' || c == '=')
            {
              q = 4;
              e = LexemType.Sign;
            }
            else
            {
              skipChar = true;
              e = LexemType.Unknown;
            }
            break;
          case 1: //letter
            if (! (char.IsDigit(c) || char.IsLetter(c) || c == '_' || c == '-'))
            {
              stop = true;
              skipChar = true;
            }
            break;
          case 2: //digits
            if (!char.IsDigit(c))
            {
              stop = true;
              skipChar = true;
            }
            break;
          case 3: //:=
            if (c == '=')
            {
              q = 4;
              e = LexemType.IsSign;
            }
            else
            {
              e = LexemType.Sign;
              stop = true; //!!! here no skip char
            }
            break;
          case 4:
            stop = true;
            skipChar = true;
            break;
          case 5: //"
            if (c == '\\')
            {
              skipChar = true;
              q = 11;
            }
            if (c == '"')
            {
              skipChar = true;
              q = 6;
            }
            break;
          case 6:
            if (c == '"')
            {
              q = 5;
            }
            else
            {
              skipChar = true;
              stop = true;
            }
            break;
          case 7:
            if (c == '*')
            {
              q = 8;
              skipChar = true;
            }
            else if (c == '/')
            {
              q = 10;
              skipChar = true;
            }
            else
            {
              sb.Append('/');
              e = LexemType.MulOp;
              stop = true;
              skipChar = true;
            }
            break;
          case 8: //waiting * in */
            if (c == '*')
            {
              q = 9;
            }
            skipChar = true;
            break;
          case 9: //waiting / in */
            if (c == '/')
            {
              q = 0;
            }
            else
            {
              q = 8;
            }
            skipChar = true;
            break;
          case 10: //waiting end of line after //
            skipChar = true;
            break;
          case 11: //waiting r, n, \ after \ in ""
            if (c == 'n')
            {
              sb.Append("\r\n");
              skipChar = true;
            }
            q = 5;
            break;
          default: //exit states
            throw new LexerInternalException("unknown state", mInput, q, i);
        }
        if (!stop)
        {
          i++;
        }
        if (!skipChar)
        {
          sb.Append(c);
        }
        else
        {
          skipChar = false;
        }
      }

      if (q == 5)
      {
        throw new GrammarSyntaxException("Illegal termial symbol, \" expected", mInput);
      }

      return (e == LexemType.Unknown) ? null : new Lexema(sb.ToString(), e, this);
    }
  }
}