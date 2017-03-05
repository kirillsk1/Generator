 
using System;
using System.Reflection;
using GraphTools;
using Microsoft.Glee.Drawing;

namespace VisualStructure
{
  public class GraphBuilder
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

    public Graph g;

    private EventHandler mUpdated = null;

    public event EventHandler Updated
    {
      add { mUpdated += value; }
      remove { mUpdated -= value; }
    }

    public void ReBuildGraph()
    {
      g = new Graph("g1");
      string t1 = mRootObject.GetType().Name;
      g.AddNode(t1);
      AddProperties(t1, mRootObject);
      //g.AddEdge("a", "b");

      if (mUpdated != null)
      {
        mUpdated(this, new EventArgs());
      }
    }

    private void AddProperties(string aRootNode, object aRootObject)
    {
      if (aRootObject == null)
        return;
      foreach (FieldInfo lField in aRootObject.GetType().GetFields())
      {
        g.AddEdge(aRootNode, "Field", lField.Name);
        if (lField.Name.StartsWith("m"))
        {
          AddProperties(lField.Name, lField.GetValue(aRootObject));
        }
      }
    }

    private int nodeid;

    public void Start()
    {
      g = new Graph("g1");
      nodeid = 0;
    }

    public string AddNode(string aNodeLabel)
    {
      nodeid++;
      INode n = g.AddNode(nodeid.ToString());
      n.NodeAttribute.Shape = Shape.Box;
      n.NodeAttribute.Label = aNodeLabel;
      return nodeid.ToString();
    }

    public string AddNodeOval(string aNodeLabel)
    {
      nodeid++;
      INode n = g.AddNode(nodeid.ToString());
      //n.NodeAttribute.Shape = Shape.Box;
      n.NodeAttribute.Label = aNodeLabel;
      return nodeid.ToString();
    }

    public string AddRedNodeOval(string aNodeLabel)
    {
      nodeid++;
      INode n = g.AddNode(nodeid.ToString());
      //n.NodeAttribute.Shape = Shape.Box;
      n.NodeAttribute.Color = Color.Red;
      n.NodeAttribute.Label = aNodeLabel;
      return nodeid.ToString();
    }

    public IEdge AddEdge(string aNodeID1, string aNodeID2)
    {
      return g.AddEdge(aNodeID1, aNodeID2);
    }
    public IEdge AddEdge(string aNodeID1, string aNodeText1 , string aNodeID2, string aNodeText2)
    {
      IEdge e = g.AddEdge(aNodeID1, aNodeID2);
      g.FindNode(aNodeID1).NodeAttribute.Label = aNodeText1;
      g.FindNode(aNodeID2).NodeAttribute.Label = aNodeText2;
      return e;
    }
    public IEdge AddEdge(tNode aNode1, tNode aNode2)
    {
      return AddEdge(aNode1.Name, aNode1.Text, aNode2.Name, aNode2.Text);
    }

    public void AddTribbleEdge(tNode aNode1, tNode aNode2)
    {
      IEdge savedG = AddEdge(aNode1, aNode2);
      savedG.EdgeAttr.Linewidth = 6;
      savedG.EdgeAttr.Color = Color.Blue;
      g.FindNode(savedG.Source).NodeAttribute.Color = Color.Blue;
      g.FindNode(savedG.Target).NodeAttribute.Color = Color.Blue;
    }

    public void AddSimpleEdge(tNode aNode1, tNode aNode2)
    {
      IEdge savedG = AddEdge(aNode1, aNode2);      
    }
    public void AddDoubleEdge(tNode aNode1, tNode aNode2)
    {    
      IEdge savedG = AddEdge(aNode1, aNode2);
      savedG.EdgeAttr.Linewidth = 3;
      savedG.EdgeAttr.Color = Color.Gray;      
    }

    public void AddPunktirEdge(tNode aNode1, tNode aNode2)
    {
      IEdge savedG = AddEdge(aNode1, aNode2);
      savedG.EdgeAttr.Color = Color.Brown;
    }

    public void AddEdge(string aNodeID1, string aLabel, string aNodeID2)
    {
      g.AddEdge(aNodeID1, aLabel, aNodeID2);
    }

    public void End()
    {
      if (mUpdated != null)
      {
        mUpdated(this, new EventArgs());
      }
    }

    public void SetBGColor(string aStr, byte ar, byte ag, byte ab)
    {
      INode lNode = g.FindNode(aStr);
      if (null != lNode)
      {
        lNode.NodeAttribute.Fillcolor = new Color(ar, ag, ab);
      }
    }
  }
}