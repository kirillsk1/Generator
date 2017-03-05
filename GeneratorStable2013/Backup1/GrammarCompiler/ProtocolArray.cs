 
using System.Collections.Generic;

namespace GrammarCompiler
{
  public class ProtocolBase
  {
  }

  public class ProtocolArray : ProtocolBase
  {
    private Dictionary<string, List<ProtocolBase>> mDic;

    public ProtocolArray()
    {
      mDic = new Dictionary<string, List<ProtocolBase>>();
    }
  }
}