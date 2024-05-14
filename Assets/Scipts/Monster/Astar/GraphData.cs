using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Node
{
    public Node(Vector3 Position) { Pos = Position; }
    public Vector3 Pos;
    public bool isOpen { get { return isopen; } }
    private bool isopen;
    public int NumberForNode = -1;

    [HideInInspector]
    public float hDistance;
    [HideInInspector]
    public float pDistance;
    [HideInInspector]
    public Node previousNode;
    

}
public class Path
{
    public Path(Node a, Node b) { a = startNode; b = endNode; }
    public Path(int a, int b) { a = nodeANum; b = nodeBNum; }

    public Node startNode;
    public Node endNode;

    public int nodeANum;
    public int nodeBNum;

    public int NumberForPath = -1;
    public bool isOneWay = false;
    public bool isOpen = true;
}
public class GraphData 
{
    public float nodeSize = 0.5f;
    public List<Node> nodes;
    public List<Path> paths;
    public Dictionary<int, Node> nodeSorted;
    public Dictionary<int, Path> PathSorted;


    public GameObject node;

    public GraphData()
    {
        nodes = new List<Node>();
        paths = new List<Path>();
        nodeSorted = new Dictionary<int, Node>();
        PathSorted = new Dictionary<int, Path>();
    }

    public Node GetNode(int i)
    {
        if(nodeSorted.ContainsKey(i)) return nodeSorted[i];
        return null;
    }
    public Path GetPath(int i)
    {
        if(PathSorted.ContainsKey(i)) return PathSorted[i];
        return null;
    }
    public Path IsCorrectPath(Node a , Node b)
    {
        if(a == null || b == null) return null;
        else return IsCorrectPath(a.NumberForNode, b.NumberForNode);
    }
    public Path IsCorrectPath(int a, int b)
    {
        foreach (var path in paths)
        {
            if ((path.nodeANum == a) && (path.nodeBNum == b) ||
                (path.nodeANum == b) && (path.nodeBNum == a))
                return path;
        }
        return null;
    }
    public void RefreshSortedDictionary()
    {
        if (nodes == null)
            return;
        int maxId = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].NumberForNode > maxId) maxId = nodes[i].NumberForNode;
        }
        maxId = maxId + 1;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].NumberForNode <= maxId) nodes[i].NumberForNode = maxId++;
        }
     
        
        maxId = 0;
        for (int i = 0; i < paths.Count; i++)
        {
            if (paths[i].NumberForPath > maxId) maxId = paths[i].NumberForPath;
        }
        maxId = maxId + 1;

        for(int i = 0;i < paths.Count; i++)
        {
            if (paths[i].NumberForPath <= maxId) paths[i].NumberForPath = maxId++;
        }


        nodeSorted.Clear();
        PathSorted.Clear();

        for (int i = 0; i < nodes.Count; i++)
        {
            nodeSorted[nodes[i].NumberForNode] = nodes[i];
        }
        for (int i = 0; i < paths.Count; i++)
        {
            PathSorted[paths[i].NumberForPath] = paths[i];
        }
    }


    //[ContextMenu("Hello")]
    //public void Something()
    //{
    //    Vector3 nextPos;
    //    for(int i = 0; i< 10; i++)
    //    {
    //        for(int j = 0; j < 10; j++)
    //        {
    //            nextPos = new Vector3(i, 0, j);
    //            GameObject instance = Instantiate(node, nextPos, Quaternion.identity);
    //            instance.transform.SetParent(transform);
    //        }
    //    }
    //}

}
