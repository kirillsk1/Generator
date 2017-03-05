 
namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет во внутреннем представлении правой части грамматики
  /// Команду вставки (точнее присвоения) в PlaceHolder выведенного текста.
  /// Например, 
  /// a := &lt;p&gt;, b
  /// b := "c", &lt;p&gt;="123"
  /// </summary>    
  public partial class PlaceHolderAssignPhrase : PlaceHolderPhrase
  {
    public readonly IPhrase RightPhrase;
    public readonly bool Add;

    public PlaceHolderAssignPhrase(Grammar aGrammar, string aName, IPhrase aRightPhrase, bool aAdd)
      : base(aGrammar, aName)
    {
      RightPhrase = aRightPhrase;
      Add = aAdd;
    }

    public override string ToString()
    {
      string result = "<" + Name + ">";
      if (Add) result += "+";
      result += "=";
      result += RightPhrase.ToString();
      return result;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }
  }
}