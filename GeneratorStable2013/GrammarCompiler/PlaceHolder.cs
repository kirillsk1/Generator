using System.Collections.Generic;

namespace GrammarCompiler
{
  public class PlaceHolderList : Dictionary<string, List<IDerivation>>
  {
    public List<IDerivation> GetList(string aName)
    {
      List<IDerivation> lList;
      if (!ContainsKey(aName))
      {
        lList = new List<IDerivation>();
        Add(aName, lList);
      }
      else
      {
        lList = this[aName];
      }
      return lList;
    }

    public void Add(string aName, IDerivation aDer)
    {
      GetList(aName).Add(aDer);
    }
  }

  partial class PlaceHolderPhrase
  {
    public static PlaceHolderList PlaceHolders = new PlaceHolderList();

    public override IDerivation Expand(DerivationContext aContext)
    {
      ListDerivation lList = new ListDerivation(true, aContext);
      TextDerivation lText = new TextDerivation(Name); //Symbol.Create(mGrammar, Name, true);
      lList.Add(lText);
      //add this point to special list for futher replacement
      PlaceHolders.Add(Name, lText);
      return lList;
    }
  }

  partial class PlaceHolderAssignPhrase
  {
    public override IDerivation Expand(DerivationContext aContext)
    {
      IDerivation lExpandList = RightPhrase.Expand(aContext);
      List<IDerivation> lReplacePoints = PlaceHolders.GetList(Name);
      foreach (TextDerivation lPoint in lReplacePoints)
      {
        if (Add)
        {
          lPoint.Text += lExpandList.ToString();
        }
        else
        {
          lPoint.Text = lExpandList.ToString();
        }
      }
      return lExpandList;
    }
  }
}