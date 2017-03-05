 
using System.IO;
using GrammarCompiler;
using NUnit.Framework;

namespace Tests
{
  public class GrammarTestRunner
  {
    private const string cPathPrefix = @"..\..\TestGrammars\";

    public void Run(string aGram)
    {
      Grammar lGr = Grammar.FromTextFile(cPathPrefix + aGram + ".txt");
      Generator lGen = new Generator(lGr);
      string lResStr = lGen.GenerateAll().Trim();

      string lResFileName = cPathPrefix + aGram + "Res.txt";
      if (File.Exists(lResFileName))
      {
        string lExpectedStr = File.ReadAllText(lResFileName).Trim();
        if (lResStr != lExpectedStr)
        {
          File.WriteAllText(cPathPrefix + aGram + "err.txt", lResStr);
          throw new AssertionException(
            string.Format(
              "test {0} failed. Go to {0}err.txt to feel the difference between your expectations and our abilities!",
              aGram));
        }
      }
      else
      {
        //save generated result
        File.WriteAllText(lResFileName, lResStr);
        throw new AssertionException(string.Format("Res file for {0} was generated automaticaly", aGram));
      }
    }

    public void Run(string aGram, string option)
    {
      Grammar lGr = Grammar.FromTextFile(cPathPrefix + aGram + ".txt");
      Generator lGen = new Generator(lGr);
      switch (option)
      {
        case "pairs":
          lGen.Options.AlternativeAlg = AlternativeSelectAlg.Pairs;
          break;
        case "enum":
          lGen.Options.AlternativeAlg = AlternativeSelectAlg.Enum;
          break;
        case "minRnd":
          lGen.Options.AlternativeAlg = AlternativeSelectAlg.MinRnd;
          break;
        case "normalDistr":
          lGen.Options.AlternativeAlg = AlternativeSelectAlg.NormalDistr;
          break;
        case "rndDistr":
          lGen.Options.AlternativeAlg = AlternativeSelectAlg.RndDistr;
          break;
        default:
          break;
      }
      string lResStr = lGen.GenerateAll().Trim();

      string lResFileName = cPathPrefix + aGram + "Res.txt";
      if (File.Exists(lResFileName))
      {
        string lExpectedStr = File.ReadAllText(lResFileName).Trim();
        if (lResStr != lExpectedStr)
        {
          File.WriteAllText(cPathPrefix + aGram + "err.txt", lResStr);
          throw new AssertionException(
            string.Format(
              "test {0} failed. Go to {0}err.txt to feel the difference between your expectations and our abilities!",
              aGram));
        }
      }
      else
      {
        //save generated result
        File.WriteAllText(lResFileName, lResStr);
        throw new AssertionException(string.Format("Res file for {0} was generated automaticaly", aGram));
      }
    }

    public string RunText(string s)
    {
      Grammar lGr = Grammar.FromText(s);
      Generator lGen = new Generator(lGr);
      return lGen.GenerateAll().Trim();
    }

    public void RunLexer(string aInFile)
    {
      string lResStr = "";
      string[] lInStr = File.ReadAllLines(cPathPrefix + aInFile + ".txt");
      foreach (string s in lInStr)
      {
        Lexer l = new Lexer(s);
        l.mDebugMode = true;
        Lexema lex;
        do
        {
          lex = l.GetNext();
        } while (lex != null);
        lResStr += "\r\n" + l.DebugText;
      }
      lResStr = lResStr.Trim();

      string lResFileName = cPathPrefix + aInFile + "Res.txt";
      if (File.Exists(lResFileName))
      {
        string lExpectedStr = File.ReadAllText(lResFileName).Trim();
        if (lResStr != lExpectedStr)
        {
          throw new AssertionException(string.Format("Lexer test {0} failed", aInFile));
        }
      }
      else
      {
        //save generated result
        File.WriteAllText(lResFileName, lResStr);
        throw new AssertionException(string.Format("Res file for {0} was generated automaticaly", aInFile));
      }
    }
  }

  [TestFixture]
  public class GrammarTest
  {
    private readonly GrammarTestRunner mRunner = new GrammarTestRunner();

    [Test]
    public void TestSeq()
    {
      mRunner.Run("Seq1");
    }

    [Test]
    public void TestAlt()
    {
      string lStr = mRunner.RunText(@"a := ""b"" | ""c""");
      if (!(lStr == "b" || lStr == "c"))
      {
        throw new AssertionException("alt result: " + lStr);
      }
    }

    [Test]
    public void TestQuant()
    {
      mRunner.Run("Quant");
    }

    [Test]
    public void TestPairs()
    {
      mRunner.Run("testPairs", "pairs");
    }

    [Test]
    public void TestPh()
    {
      mRunner.Run("PlaceHolder");
    }

    [Test]
    public void TestAddFuncs()
    {
      mRunner.Run("AddFuncs");
    }

    [Test]
    public void TestSep()
    {
      mRunner.Run("Sep");
    }

    [Test]
    public void TestExpr()
    {
      mRunner.Run("Expr");
    }

    [Test]
    public void TestIf()
    {
      mRunner.Run("If");
    }

    [Test]
    public void TestListTrans()
    {
      mRunner.Run("ListTrans");
    }

    [Test]
    public void TestLexer()
    {
      mRunner.RunLexer("Lexer");
    }

    [Test]
    public void TestLexerComment()
    {
      mRunner.RunLexer("LexerComment");
    }

    [Test]
    //[ExpectedException("GrammarSynaxException", "unexpected end of rule")]
    public void UnexpectedEndAfterCommaTest()
    {
      Grammar G = Grammar.FromText(@"a := ""b"", ""c"",");
      if (G.SyntaxErrors.Count == 0)
      {
        throw new AssertionException("err not detected");
      }
    }
  }

  [TestFixture]
  public class TransductorTest
  {
    private readonly GrammarTestRunner mRunner = new GrammarTestRunner();

    [Test]
    public void RndTest()
    {
      string lStr = mRunner.RunText("a := Rnd(10)");
      int i = int.Parse(lStr);
      if (i < 0 || i > 10)
      {
        throw new AssertionException("rnd err");
      }
    }

    [Test]
    public void RndMinMaxTest()
    {
      string lStr = mRunner.RunText("a := Rnd(10, 15)");
      int i = int.Parse(lStr);
      if (i < 10 || i > 15)
      {
        throw new AssertionException("rnd min max err");
      }
    }
  }
}