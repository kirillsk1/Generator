 
using System;
using System.Threading;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  /// <summary>
  ///   /// Грамматика по которой задаются правила:
  /// Phrase := Alt, Alts 
  /// Alt    := Seq, [AS Alias], Seqs 
  /// Alts   := "|", Alt, Alts
  ///           | ""
  /// Seq    := (T | NT),[Quant]
  ///                  | "(",Phrase,")",[Quant]
  ///                  | "[",Phrase,"]"
  ///                  | "{",Phrase,"}"
  ///                  | "<",PlaceHolderName,">", [PlaceHolderAssign], [Quant]
  ///                  | TransCall, [Quant]
  ///                  | #Expr
  /// Seqs   := ",", Seq, [AS Alias] , Seqs
  ///           | "" 
  ///
  /// Quant  := "{", int, ["..",int | "*"], "}"
  /// TransCall := [NT.]NT, "(", [Param, Parms], ")", [Acc]
  /// Param := Expr //n | T //числовые или строковые параметры, ? или %var%
  /// Params := ",", Param, Params
  ///           | ""
  /// PlaceHolderAssign := "=", NT | "+=", NT
  /// 
  /// Встроенные Выражения
  /// 
  /// Expr     := AddExpr, AddExprs
  /// AddExpr  := MulExpr, MulExprs
  /// AddExprs := ("+"|"-"), AddExpr, AddExprs
  ///             | ""
  /// MulExprs := ("*" | "/"), MulExpr, MulExprs
  /// MulExpr  := n
  ///             | "(", Expr, ")"
  ///             | NT, Accs
  ///             | T
  ///             | TransCall
  /// Accs     := Acc, Accs | ""
  /// Acc      := "[", Expr, "]"
  ///             | ".", NT
  /// 
  /// Будем разбирать методом рекурсивного спуска
  /// 
  /// </summary>
  public class PhraseParser
  {
    #region fileds & ctor

    protected Lexema lex;
    protected Grammar mGrammar;

    public PhraseParser(Grammar aGrammar, Lexema aLex)
    {
      mGrammar = aGrammar;
      lex = aLex;
    }

    #endregion

    #region Grammar core (Phrase, Alt, Seq)

    public IPhrase ParseRule()
    {
      IPhrase lPhr = ParsePhrase();
      if (lex != null)
      {
        throw new GrammarSyntaxException(string.Format("rule parse error! {0} found", lex.Text));
      }
      return lPhr;
    }

    private IPhrase ParsePhrase()
    {
      if (lex == null)
      {
        throw new GrammarSyntaxException("Phrase expected");
      }
      IPhrase lAlt = Alt();
      Alts(ref lAlt);
      return lAlt;
    }

    private IPhrase Alt()
    {
      IPhrase lPhr = Seq();
      AsAlias(ref lPhr);
      Seqs(ref lPhr);
      return lPhr;
    }

    private void AsAlias(ref IPhrase aPhr)
    {
      if (null != lex)
      {
        if (lex.Type == LexemType.NotTerminal && lex.Text.ToLower() == "as")
        {
          lex = lex.Next();
          CheckType(LexemType.NotTerminal);
          aPhr.Alias = lex.Text;
          lex = lex.Next();
        }
      }
    }

    //  Alts   := "|", Alt, Alts
    //           | ""
    private void Alts(ref IPhrase aPhr)
    {
      if (lex != null)
      {
        if (lex.Type == LexemType.Operation && lex.Text == "|")
        {
          lex = lex.Next();
          AlternativeSet lAlt = aPhr as AlternativeSet;
          if (lAlt == null)
          {
            lAlt = new AlternativeSet(mGrammar, aPhr);
            aPhr = lAlt;
          }
          lAlt.Add(Alt()); //ParsePhrase());
          Alts(ref aPhr);
        }
        //else
        //{
        //  throw new GrammarSynaxException(string.Format("'{0}' not expected here (|) operation expected",lex.Text));
        //}
      }
    }

    //Seq    := T | NT | "(",Phrase,")"
    private IPhrase Seq()
    {
      if (lex == null)
      {
        throw new GrammarSyntaxException("unexpected end of rule");
      }
      IPhrase lPhr;
      if (lex.Type == LexemType.LeftBr)
      {
        lex = lex.Next();
        lPhr = ParsePhrase();
        Expect(LexemType.RightBr, ")");
        Quant(ref lPhr);
      }
      else if (lex.Type == LexemType.Sign && lex.Text == "[")
      {
        lex = lex.Next();
        lPhr = new QuantifiedPhrase(mGrammar, ParsePhrase(), 0, 1);
        ExpectSign("]");
      }
      else if (lex.Type == LexemType.Sign && lex.Text == "{")
      {
        lex = lex.Next();
        lPhr = new QuantifiedPhrase(mGrammar, ParsePhrase());
        ExpectSign("}");
      }
      else if (lex.Type == LexemType.Sign && lex.Text == "<")
      {
        lex = lex.Next();
        if (lex.Type != LexemType.NotTerminal)
        {
          throw new GrammarSyntaxException("Name expected");
        }
        string lName = lex.Text;
        lex = lex.Next();
        ExpectSign(">");
        lPhr = PlaceHolderAssign(lName);
        Quant(ref lPhr);
      }
      else if (lex.Text == "#")
      {
        lex = lex.Next();
        lPhr = Expr();
      }
      else
      {
        //терминальный или не терминальный
        if (lex.Type == LexemType.Terminal)
        {
          lPhr = new Terminal(mGrammar, lex.Text);
          lex = lex.Next();
        }
        else if (lex.Type == LexemType.NotTerminal)
        {
          lPhr = TransCall();
        }
        else
        {
          throw new GrammarSyntaxException("Terminal or Non-Terminal symbol expected");
        }
        Quant(ref lPhr);
      }

      return lPhr;
    }

    #endregion

    //Returns simple Nonterminal symbol or TransCallPhrase
    private IPhrase TransCall()
    {
      string Name = lex.Text;
      lex = lex.Next();
      if (lex != null && lex.Type == LexemType.LeftBr)
      {
        lex = lex.Next();
        return TransCall1(null, Name);
      }
      return NonTerminal.Create(mGrammar, null, Name); //TODO!!!
    }

    //lex: Rnd( ^ [param])
    private PhraseBase TransCall1(Access aBaseAcc, string aTransName)
    {
      TransCallPhrase lPhr = new TransCallPhrase(mGrammar, aTransName, aBaseAcc);
      if (lex.Text != ")")
      {
//here is at least 1 param - parse it
        TransParam(ref lPhr);
        TransParams(ref lPhr);
      }
      Expect(LexemType.RightBr, ")");
      lPhr.DoBinding();
      IExpr lExpr = lPhr;
      if (Acc(ref lExpr))
      {
        return (PhraseBase) lExpr;
      }
      return lPhr;
    }


    private void TransParam(ref TransCallPhrase aPhr)
    {
      aPhr.AddParametr(Expr());
      /*
      //determine param type
      if (lex.Type == LexemType.Terminal)
      {//string
        aPhr.AddParametr(new TextDerivation(lex.Text));
        lex = lex.Next();
      }
      else
      {//int
        aPhr.AddParametr(ParseIntValue());
      }
       */
    }

    private void TransParams(ref TransCallPhrase aPhr)
    {
      if (lex.Type == LexemType.Operation && lex.Text == ",")
      {
        lex = lex.Next();
        TransParam(ref aPhr);
        TransParams(ref aPhr);
      }
    }

    // Seqs   := ",", Seq, Seqs
    //           | "" 
    private void Seqs(ref IPhrase aPhr)
    {
      if (lex != null)
      {
        if (lex.Type == LexemType.Operation)
        {
          if (lex.Text == "," || lex.Text == ";")
          {
            bool lIsComma = lex.Text == ",";
            lex = lex.Next();
            Seqence lSeq = aPhr as Seqence;
            if (lSeq == null)
            {
              lSeq = new Seqence(mGrammar, aPhr);
              lSeq.InsertSpace = lIsComma;
              aPhr = lSeq;
            }
            IPhrase lSeqPhr = Seq();
            lSeq.Add(lSeqPhr);
            AsAlias(ref lSeqPhr);
            Seqs(ref aPhr);
          }
        }
        //else
        //{
        //  throw new GrammarSynaxException(string.Format("'{0}' not expected here (,) operation expected", lex.Text));
        //}
      }
    }

    private void Quant(ref IPhrase aPhr)
    {
      // look for quantifier here
      if (lex != null && (lex.Text == "{"
                          || lex.Text == "*"
                          || lex.Text == "+"
                          || lex.Text == "?"))
      {
        ExprInt lMin = null, lMax = null;
        //{1..5}
        if (lex.Text == "{")
        {
          lex = lex.Next();
          lMin = ParseIntValue();
          if (lex.Text == ".")
          {
            lex = lex.Next();
            ExpectSign(".");
            if (lex.Text == "*")
            {
              lMax = new ExprInt(Int32.MaxValue);
            }
            else
            {
              lMax = ParseIntValue();
            }
          }
          else
          {
            lMax = lMin;
          }
          ExpectSign("}");
        }
        else if (lex.Text == "*")
        {
          lMin = new ExprInt(0);
          lMax = new ExprInt(int.MaxValue);
          lex = lex.Next();
        }
        else if (lex.Text == "+")
        {
          lMin = new ExprInt(1);
          lMax = new ExprInt(int.MaxValue);
          lex = lex.Next();
        }
        else if (lex.Text == "?")
        {
          lMin = new ExprInt(0);
          lMax = new ExprInt(1);
          lex = lex.Next();
        }
        else
        {
          throw new GrammarSyntaxException("Недолжно быть. Неподдерживаемый знак после нетерминала " + lex.Text);
        }
        QuantifiedPhrase lQuant = new QuantifiedPhrase(mGrammar, aPhr, lMin, lMax);
        aPhr = lQuant;
      }
    }

    private PlaceHolderPhrase PlaceHolderAssign(string aName)
    {
      if (lex.Type == LexemType.Sign)
      {
        if (lex.Text == "=")
        {
          lex = lex.Next();
          IPhrase lPhr = Seq();
          AsAlias(ref lPhr);
          return new PlaceHolderAssignPhrase(mGrammar, aName, lPhr, false);
        }
        else if (lex.Text == "+")
        {
          lex = lex.Next();
          IPhrase lPhr = Seq();
          AsAlias(ref lPhr);
          return new PlaceHolderAssignPhrase(mGrammar, aName, lPhr, true);
        }
      }
      return new PlaceHolderPhrase(mGrammar, aName);
    }

    #region Expressions

    private IExpr Expr()
    {
      if (lex == null)
      {
        throw new GrammarSyntaxException("Builtin Expression expected");
      }
      IExpr lAddExpr = AddExpr();
      AddExprs(ref lAddExpr);
      return lAddExpr;
    }

    private void AddExprs(ref IExpr aExpr)
    {
      if (lex != null)
      {
        if (lex.Type == LexemType.AddOp)
        {
          eOperation lOp = ExprOp.ParseOperation(lex.Text);
          lex = lex.Next();
          ExprOp lExprOp = aExpr as ExprOp;
          if (lExprOp == null || lExprOp.Operation != lOp)
          {
            lExprOp = new ExprOp(lOp);
            lExprOp.AddOperand(aExpr);
            aExpr = lExprOp;
          }
          lExprOp.AddOperand(Expr());
          AddExprs(ref aExpr);
        }
      }
    }

    private IExpr AddExpr()
    {
      IExpr lExpr = MulExpr();
      MulExprs(ref lExpr);
      return lExpr;
    }

    private IExpr MulExpr()
    {
      IExpr lRes;
      if (lex.Type == LexemType.LeftBr)
      {
        lex = lex.Next();
        lRes = Expr();
        Expect(LexemType.RightBr, ")");
      }
      else if (lex.Type == LexemType.NotTerminal)
      {
        string lName = lex.Text;
        lex = lex.Next();
        if (lex != null && lex.Type == LexemType.LeftBr)
        {
          lex = lex.Next();
          lRes = (IExpr) TransCall1(null, lName);
        }
        else
        {
          IExpr lAcc = new Access(mGrammar, lName);
          Accs(ref lAcc);
          return lAcc;
        }
      }
      else
      {
        lRes = ParseExprValue();
      }
      return lRes;
    }

    /// <summary>
    /// Can return ExprInt, ExprDouble, ExprString or variable
    /// 1, 1.2, "aaa", %VARNAME%
    /// </summary>
    /// <returns></returns>
    private IExpr ParseExprValue()
    {
      IExpr lRes;
      if (lex.Type == LexemType.Terminal)
      {
        lRes = new ExprString(lex.Text);
        lex = lex.Next();
      }
      else
      {
        string lIntStr = lex.Text;
        lRes = ParseIntValue();
        //check if doble
        if (lex != null && lex.Text == ".")
        {
          lex = lex.Next();
          CheckType(LexemType.IntNumber);
          Double d =
            double.Parse(lIntStr
                         + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator
                         + lex.Text);
          lex = lex.Next();
          lRes = new ExprDouble(d);
        }
      }
      return lRes;
    }

    private ExprInt ParseIntValue()
    {
      //check vars
      if (lex.Type == LexemType.Sign && lex.Text == "%")
      {
        //variable
        lex = lex.Next();
        CheckType(LexemType.NotTerminal);
        string lVarName = lex.Text;
        ExpectSign("%");
        throw new NotImplementedException();
        //return new ExprConfVariable(lVarName);
      }
      else
      {
        //number
        CheckType(LexemType.IntNumber);
        int r = int.Parse(lex.Text);
        lex = lex.Next();
        return new ExprInt(r);
      }
    }

    private void Accs(ref IExpr lBaseAcc)
    {
      if (lex != null)
      {
        if (Acc(ref lBaseAcc))
        {
          Accs(ref lBaseAcc);
        }
      }
    }

    /// <summary>
    /// Parses Acc to array or seq ([], .)
    /// and ListTrans call e.g. Param.Count()
    /// </summary>
    /// <param Name="lBaseAcc"></param>
    /// <returns>true - access found, continue chain of accs, false - stop parsing</returns>
    private bool Acc(ref IExpr lBaseAcc)
    {
      //arr
      if (null != lex)
      {
        if (lex.Text == "[")
        {
          lex = lex.Next();
          IExpr lIndExpr = Expr();
          ExpectSign("]");
          lBaseAcc = new AccessArray(mGrammar, (Access) lBaseAcc, lIndExpr);
          return true;
        }

        //struct
        if (lex.Text == ".")
        {
          lex = lex.Next();
          string lFieldName = lex.Text;
          lex = lex.Next();
          if (lex != null && lex.Type == LexemType.LeftBr)
          {
// call of ListTrans
            lex = lex.Next();
            lBaseAcc = (IExpr) TransCall1((Access) lBaseAcc, lFieldName);
          }
          else
          {
            lBaseAcc = new AccessSeq(mGrammar, lBaseAcc, lFieldName);
            return true;
          }
        }
      }
      return false;
    }

    private void MulExprs(ref IExpr aExpr)
    {
      if (lex != null)
      {
        if (lex.Type == LexemType.MulOp)
        {
          eOperation lOp = ExprOp.ParseOperation(lex.Text);
          lex = lex.Next();
          ExprOp lExprOp = aExpr as ExprOp;
          if (lExprOp == null || lExprOp.Operation != lOp)
          {
            lExprOp = new ExprOp(lOp);
            lExprOp.AddOperand(aExpr);
            aExpr = lExprOp;
          }
          lExprOp.AddOperand(Expr());
          MulExprs(ref aExpr);
        }
      }
    }

    #endregion

    #region Lexer helper functions

    private void Expect(LexemType aType, string aText)
    {
      if (lex.Type != aType || lex.Text != aText)
      {
        throw new GrammarSyntaxException(string.Format("{0} expexted, but {1} found", aText, lex.Text));
      }
      lex = lex.Next();
    }

    private void ExpectSign(string aText)
    {
      Expect(LexemType.Sign, aText);
    }

    private void CheckType(LexemType aLexemType)
    {
      if (null == lex)
      {
        throw new GrammarSyntaxException("Unexpected end of rule");
      }
      if (lex.Type != aLexemType)
      {
        throw new GrammarSyntaxException(string.Format("{0} expected, but found {2} {1}", aLexemType, lex.Text, lex.Type));
      }
    }

    #endregion
  }
}