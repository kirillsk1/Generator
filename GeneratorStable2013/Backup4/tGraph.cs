 
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphTools
{
  public class tGraph
  {
    public Dictionary<string, tNode> Nodes;
    public List<tEdge> Edges;

    public tGraph()
    {
      Nodes = new Dictionary<string, tNode>();
      Edges = new List<tEdge>();
    }

    public tGraph(tGraph aSourceGraph) : this()
    {
      // Пройти по всем вершинам исх графа, добавить в это. Пройти по всем дугам, добавить в этот
      foreach (string nodeName in  aSourceGraph.Nodes.Keys)
      {
        addNode(nodeName);
      }
      foreach (tEdge mEdges in aSourceGraph.Edges)
      {
        addEdge(mEdges.fromNode.Name, mEdges.toNode.Name);
      }
    }

    public bool isAchieved(string aFromNode, string aToNode)
    {
      tNode targetNode = Nodes[aToNode];
      if (Nodes[aFromNode] == Nodes[aToNode])
      {
        return false;
      }
      List<tNode> visitCandidates = new List<tNode>();
      // проинициализируем все метки в -1
      foreach (tNode curNode in Nodes.Values)
      {
        curNode.labelWeight = -1;
      }
      visitCandidates.Add(Nodes[aFromNode]);
      Nodes[aFromNode].labelWeight = 0;
      while (visitCandidates.Count > 0)
      {
        tNode curNode = visitCandidates[0];
        int nextLabel = curNode.labelWeight + 1;
        foreach (tEdge curEdge in curNode.outEdges)
        {
          if (curEdge.toNode.labelWeight == -1)
          {
            curEdge.toNode.labelWeight = nextLabel;
            visitCandidates.Add(curEdge.toNode);
            if (curEdge.toNode == targetNode)
            {
              return true;
            }
          }
          else
          {
            if (nextLabel < curEdge.toNode.labelWeight)
            {
              curEdge.toNode.labelWeight = nextLabel;
            }
          }
        }
        visitCandidates.Remove(curNode);
      }
      return false;
    }

    //Отслеживает путь между вершинами. 
    //По меткам, оставшимся после isAchieved, двигаясь обратно от ToNode
    public List<string> GetPath(string aFromNode, string aToNode)
    {
      //проверочки
      if (!Nodes.ContainsKey(aToNode))
      {
        throw new Exception("неправильное имя узла " + aToNode);
      }
      if (!Nodes.ContainsKey(aFromNode))
      {
        throw new Exception("неправильное имя узла " + aToNode);
      }
      //будем вставлять в начало этого списка вершины,
      //двигаясь от конца к началу пути, пока не вставим aFromNode
      List<string> lResult = new List<string>();
      lResult.Add(aToNode);
      tNode CurTargetNode = Nodes[aToNode];
      tNode FromNode = Nodes[aFromNode];
      while (CurTargetNode != FromNode)
      {
        tNode PrevNode = null;
        int prevLabel = CurTargetNode.labelWeight - 1;
        foreach (tEdge e in CurTargetNode.inEdges)
        {
          if (e.fromNode.labelWeight == prevLabel)
          {
            PrevNode = e.fromNode;
            break;
          }
        }
        if (PrevNode == null)
        {
          throw new Exception("нету пути до узла " + CurTargetNode.Name);
        }
        lResult.Insert(0, PrevNode.Name);
        CurTargetNode = PrevNode;
      }
      return lResult;
    }

    /// <summary>
    /// Добавляет вершину с уникальным именем.
    /// Исключения:
    ///   Exception Не совпадает текст
    /// </summary>
    /// <param name="aName">уникальное имя</param>
    /// <param name="aText">надпись</param>
    public void ensureExistsNode(string aName, string aText)
    {
      if (Nodes.ContainsKey(aName))
      {
        if (Nodes[aName].Text != aText)
          throw new Exception(string.Format("Не совпадает текст. вершина {0} уже существует, но ее надпись {1}, а не {2}", aName, Nodes[aName].Text, aText));
      }
      else
      {
        tNode n = new tNode(aName);
        n.Text = aText;
        Nodes.Add(aName, n);
      }
    }

    /// <summary>
    /// Добавляет вершину с уникальным именем.
    /// Исключения:
    ///   Exception("Такая вершина уже существует");
    /// </summary>
    /// <param name="aName">уникальное имя</param>
    /// <returns>вершина, гарантированно не null</returns>
    public tNode addNode(string aName)
    {
      if (Nodes.ContainsKey(aName))
      {
        throw new Exception("Такая вершина уже существует");
      }
      else
      {
        tNode n = new tNode(aName);
        Nodes.Add(aName, n);
        return n;
      }
    }

    /// <summary>
    /// Добавляет вершину с заданным текстом, который не обязан быть уникальным.
    /// Этот метод позаботится о создании уникального имени для вершины
    /// </summary>
    /// <param name="aText">надпись</param>
    /// <returns>вершина, гарантированно не null</returns>
    public tNode addNonUniqueNode(string aText)
    {
      int cnt = 0;
      string name = aText;
      do
      {
        if (cnt > 0)
        {
          name = aText + cnt;
        }
        cnt++;
      } while (Nodes.ContainsKey(name));
      
      tNode n = new tNode(name);
      n.Text = aText;
      Nodes.Add(name, n);
      return n;
    }

    /// <summary>
    /// Добавляет дугу
    /// </summary>
    /// <param name="fromName"></param>
    /// <param name="toName"></param>
    /// <returns>может быть null, если соотв. вершин нет.</returns>
    public tEdge addEdge(string fromName, string toName)
    {
      if (string.IsNullOrEmpty(fromName) || string.IsNullOrEmpty(toName))
      {
        throw new ArgumentNullException("fromName or toName");
      }
      //проверить, сущ ли в словаре
      if (Nodes.ContainsKey(fromName) && Nodes.ContainsKey(toName))
      {
        return addEdge(Nodes[fromName], Nodes[toName]);
      }
      return null;
    }

    private tEdge addEdge(tNode fromNode, tNode toNode)
    {
      if (fromNode == null || toNode == null) throw new ArgumentNullException();
      //tEdge exists = FindEdge(fromNode.Name, toNode.Name);
      //if (null == exists)
      //{
        tEdge e = new tEdge(fromNode, toNode);
        Edges.Add(e);
        fromNode.outEdges.Add(e);
        toNode.inEdges.Add(e);
        return e;
      //}
      //return exists;
    }

    private tEdge FindEdge(string aFromName, string aToName)
    {
      foreach (tEdge e in Edges)
      {
        if (e.fromNode.Name == aFromName && e.toNode.Name == aToName)
        {
          return e;
        }
      }
      return null;
    }

    public tGraph findCycles()
    {
      tGraph g = new tGraph(this);
      g.removeNotCycle();
      return g;
    }

    private void removeNotCycle()
    {
      bool isChanged = true;
      while (isChanged)
      {
        isChanged = false;
        // foreach (tNode curNode in Nodes.Values)
        for (int i = 0; i < Nodes.Count; i++)
        {
          tNode curNode = Nodes.Values.ElementAt(i);
          if (curNode.inEdges.Count == 0 || curNode.outEdges.Count == 0)
          {
            removeNode(curNode);
            isChanged = true;
          }
        }
      }
    }

    private void removeNode(tNode aNode)
    {
      while (aNode.inEdges.Count > 0)
      {
        removeEdge(aNode.inEdges[0]);
      }
      while (aNode.outEdges.Count > 0)
      {
        removeEdge(aNode.outEdges[0]);
      }
      Nodes.Remove(aNode.Name);
    }

    private void removeEdge(tEdge aEdge)
    {
      aEdge.fromNode.outEdges.Remove(aEdge);
      aEdge.toNode.inEdges.Remove(aEdge);
      Edges.Remove(aEdge);
    }

    public bool ContainsEdge(tEdge aEdge)
    {
      foreach (tEdge lEdge in Edges)
      {
        if (lEdge.fromNode.Name == aEdge.fromNode.Name && lEdge.toNode.Name == aEdge.toNode.Name)
        {
          return true;
        }
      }
      return false;
    }
  }

  /// <summary>
  /// Вершина
  /// </summary>
  public class tNode
  {
    /// <summary>
    /// Имена должны быть уникальны
    /// </summary>
    public string Name;
    /// <summary>
    /// Надпись может повторятья
    /// </summary>
    public string Text;
    /// <summary>
    /// за сколько ходов достижима эта вершина
    /// </summary>
    internal int labelWeight; 
    public List<tEdge> inEdges;
    public List<tEdge> outEdges;
    /// <summary>
    /// Можно пометить вершину произвольными атрибутами в зависимости от задачи
    /// </summary>
    public object CustomAttributes;

    public tNode(string aName)
    {
      Name = aName;
      Text = Name;
      inEdges = new List<tEdge>();
      outEdges = new List<tEdge>();
    }
  }

  /// <summary>
  /// Дуга
  /// </summary>
  public class tEdge
  {
    public tNode fromNode;
    public tNode toNode;
    /// <summary>
    /// Можно пометить дугу произвольными атрибутами в зависимости от задачи
    /// </summary>
    public object CustomAttributes;

    public tEdge(tNode aFromNode, tNode aToNode)
    {
      fromNode = aFromNode;
      toNode = aToNode;
    }
  }
}