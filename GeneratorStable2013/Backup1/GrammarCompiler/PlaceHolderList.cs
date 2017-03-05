 
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
}