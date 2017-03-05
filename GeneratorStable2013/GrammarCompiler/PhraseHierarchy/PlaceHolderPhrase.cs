 
namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет PlaceHolder (Зарезервированное место для последующей вставки текста) во внутреннем представлении правой части грамматики
  /// Например, 
  /// a := &lt;p&gt;, b
  /// b := "c", &lt;p&gt;="123"
  /// </summary>  
  public class PlaceHolderPhrase : PhraseBase
  {
    public readonly string Name;

    public PlaceHolderPhrase(Grammar aGrammar, string aName)
      : base(aGrammar)
    {
      Name = aName;
    }

    public override string ToString()
    {
      return "<" + Name + ">";
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }
}