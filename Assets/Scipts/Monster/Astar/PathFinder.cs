using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using Unity.VisualScripting;
using Unity.XR.Oculus.Input;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.UIElements;



public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;

    private bool wayToElevator;

    Elevator[] elevators;
    Dictionary<float, Transform> wayToPathDic;

    public GraphData graphData = new GraphData();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        graphData.RefreshSortedDictionary();
    }
    //public void AblePath(Path path)
    //{
    //    if (path == null)
    //    {
    //        Debug.LogError(path +" (Path) not Found");
    //        return;
    //    }
    //    if(!path.isOpen) return;

    //}

    public int FindNearNode(Vector3 dir)
    {
        float minDistance = float.MaxValue;
        Node nearNode = null;
        dir = new Vector3(dir.x,0,dir.z);

        foreach(Node node in graphData.nodeSorted.Values)
        {
            float distance = Vector3.Distance(dir, node.Pos);
            if(distance < minDistance)
            {
                nearNode = node;
                minDistance = Vector3.Distance(dir, node.Pos);
            }
        }
        return nearNode != null ? nearNode.NumberForNode : -1;
    }
    public void FindShortPathToPos(Node transformPosition,Node targetPosition, System.Action<List<Node>> callback)
    {
        Node nodeA = transformPosition;
        
        if(!nodeA.isOpen)
        {
            foreach (Path path in graphData.paths)
            {
                if(path.nodeA == nodeA || path.nodeB == nodeA) nodeA = path.nodeA == nodeA ? path.nodeB : path.nodeA;
            }
        }
        else
            FindShortPath(nodeA, targetPosition,callback);
    }
    private void FindShortPath(Node node, Node nNode , System.Action<List<Node>> callback) => StartCoroutine(FindShortestPath(node,nNode, callback));


    protected IEnumerator FindShortestPath(Node fromNode, Node toNode , System.Action<List<Node>> callback)
    {
        bool found = false;
        List<Node> completeNodes = new List<Node>();
        List<Node> nextNodes = new List<Node>();
        List<Node> finalPath = new List<Node>();

        fromNode.pDistance = 0;
        fromNode.hDistance = Vector3.Distance(fromNode.Pos, toNode.Pos);
        nextNodes.Add(fromNode);
        
        foreach (var point in graphData.nodes)
        {
            point.hDistance = -1;
            point.previousNode = null;
        }
        while (true)
        {
            Node leastCostNode = null;
            float minDistance = 99999;

            foreach (Node node in nextNodes)
            {
                if (node.hDistance <= 0) node.hDistance = Vector3.Distance(node.Pos, toNode.Pos);

                if (minDistance > node.CombinedDistance) 
                {
                    leastCostNode = node;
                    minDistance = node.CombinedDistance;
                }
            }
            if (leastCostNode == null) break;

            if (leastCostNode == toNode)
            {
                found = true;
                Node prevPoint = leastCostNode;
                while (prevPoint != null)
                {
                    finalPath.Insert(0, prevPoint);
                    prevPoint = prevPoint.previousNode;
                }
                callback(finalPath);
                yield break;
            }
            yield return null;
            foreach (Path path in graphData.paths)
            {
                if (path.nodeA == leastCostNode|| path.nodeB == leastCostNode)  if (!path.isOpen) continue;

                Node otherNode = path.nodeA == leastCostNode? path.nodeB : path.nodeA;

                otherNode.previousNode = null;
                otherNode.pDistance = 0;
                otherNode.hDistance = 0;

                if (!otherNode.isOpen) continue;
                if (completeNodes.Contains(otherNode)) continue;

                if (otherNode.hDistance <= 0)
                    otherNode.hDistance = Vector3.Distance(otherNode.Pos, toNode.Pos) + Vector3.Distance(otherNode.Pos, fromNode.Pos) ;
                if (nextNodes.Contains(otherNode))
                {
                    if (otherNode.pDistance >leastCostNode.pDistance)
                    {
                        otherNode.pDistance = leastCostNode.pDistance;
                        otherNode.previousNode = leastCostNode;
                    }
                }
                else
                {
                    otherNode.pDistance= leastCostNode.pDistance;
                    otherNode.previousNode = leastCostNode;
                    nextNodes.Add(otherNode);
                }
            }
            nextNodes.Remove(leastCostNode);
            completeNodes.Add(leastCostNode);
            
            leastCostNode.previousNode = null;
            leastCostNode.hDistance = -1; // leastCostNode 다시 초기화 해주기            
            yield return null;
        }
        callback(null);
    }
}
