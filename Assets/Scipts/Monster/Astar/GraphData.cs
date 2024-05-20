using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Node
{
    public Vector3 Pos;
    public void SetPosition(Vector3 dir) => Pos = dir;
    
    public bool isOpen = true;

    public int NumberForNode = -1;
    
    [HideInInspector]
    public float hDistance;
    [HideInInspector]
    public float pDistance;
    [HideInInspector]
    public float CombinedDistance { get { return pDistance + hDistance; } }
    [HideInInspector]
    public Node previousNode;

}

[System.Serializable]
public class Path
{
    public Path(Node a,Node b) {
        nodeA =a ; 
        nodeB=b;
        nodeANum = a.NumberForNode;
        nodeBNum = b.NumberForNode;
    }
    public Path(int a, int b)
    {
        nodeANum= a;
        nodeBNum= b;
    }

    public int nodeANum;
    public int nodeBNum;
    [HideInInspector]
    public Node nodeA;
    [HideInInspector]
    public Node nodeB;

    public int NumberForPath = -1;
    public bool isOneWay = false;
    public bool isOpen = true;
}

[System.Serializable]
public class GraphData 
{
    public Color lineColor = Color.green;
    public float nodeSize = 0.5f;
    public float heigtFromGround = 1;
    public string WhatIsGroundTagName = "Ground";
    public List<Node> nodes;
    public List<Path> paths;

    [HideInInspector]
    public Dictionary<int, Node> nodeSorted;
    [HideInInspector]
    public Dictionary<int, Path> PathSorted;
    //\Assets\QPathFinder\Script\Editor\PathFinderEditor.cs

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
        maxId = maxId + 1;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].NumberForNode <= maxId) nodes[i].NumberForNode = maxId++;
        }
     
        
        maxId = 0;
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




}
