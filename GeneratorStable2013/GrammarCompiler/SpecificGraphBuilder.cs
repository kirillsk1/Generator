 
using System;
using VisualStructure;

namespace GrammarCompiler
{
  /// <summary>
  /// GraphBuilder will use reflection and xml configs
  /// but it SpecificGraphBuilder will use ListDerivation and so on directly
  /// </summary>
  public class SpecificGraphBuilder
  {
    private object mRootObject;

    public object RootObject
    {
      get { return mRootObject; }
      set
      {
        mRootObject = value;
        ReBuildGraph();
      }
    }

    public GraphBuilder gb;

    public SpecificGraphBuilder(GraphBuilder agb)
    {
      gb = agb;
    }

    public void ReBuildGraph()
    {
      gb.Start();
      AddObject(mRootObject, "root", null);
      gb.End();
    }

    private string AddObject(object aObject, string aName, string aRootNodeID)
    {
      if (aObject == null)
        return "null!";
      string type = aObject.GetType().Name;
      string title = type + " " + aName;
      string lNodeId;
      if (typeof (ListDerivation) == aObject.GetType())
      {
        ListDerivation l = aObject as ListDerivation;
        string s = "\r\n0";
        for (int i = 1; i < l.mList.Count; i++)
        {
          s += " | " + i.ToString();
        }
        lNodeId = gb.AddNode(title + s);
        gb.SetBGColor(lNodeId, 255, 255, 0);
        if (aRootNodeID != null)
        {
          gb.AddEdge(aRootNodeID, aName, lNodeId);
        }
        for (int i = 0; i < l.mList.Count; i++)
        {
          AddObject(l.mList[i], string.Format("[{0}]", i), lNodeId);
        }
        return lNodeId;
      }
      else if (typeof (TextDerivation) == aObject.GetType())
      {
        if (aRootNodeID != null)
        {
          TextDerivation l = aObject as TextDerivation;
          string lStr = string.Format("\"{0}\"", l.Text);
          gb.AddEdge(aRootNodeID, aName, lStr);
          gb.SetBGColor(lStr, 0, 255, 0);
        }
        return null;
      }
      else if (typeof (SymbolDerivation) == aObject.GetType())
      {
        SymbolDerivation l = aObject as SymbolDerivation;
        gb.AddEdge(aRootNodeID, aName, l.Symbol.Text + " " + type);
        return null;
      }
      else if (typeof (DictionaryDerivation) == aObject.GetType())
      {
        DictionaryDerivation l = aObject as DictionaryDerivation;
        String[] keys = l.Keys;
        title += "\r\n" + string.Join("\r\n", keys);
        lNodeId = gb.AddNode(title);
        gb.SetBGColor(lNodeId, 255, 128, 0);
        if (aRootNodeID != null)
        {
          gb.AddEdge(aRootNodeID, aName, lNodeId);
        }
        foreach (string lKey in keys)
        {
          AddObject(l[lKey], lKey, lNodeId);
        }
        return lNodeId;
      }
      //default
      lNodeId = gb.AddNode(title);
      return lNodeId;
    }
  }
}