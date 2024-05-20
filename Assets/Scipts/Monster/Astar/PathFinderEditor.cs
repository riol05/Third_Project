using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinderEditor : MonoBehaviour
{
    public PathFinder script;

    public LayerMask Wall;
    private RaycastHit hit;
    [ContextMenu("Refresh")]
    public void refresh()
    {
        script.graphData.RefreshSortedDictionary();
    }


    [ContextMenu("Instantiate Node")]
    public void MakeNodeOnField()
    {
        Vector3 nextPos;
        HashSet<Vector3> nodePositions = new HashSet<Vector3>();

        if (script.graphData.nodes != null)
        {
            foreach (Node node in script.graphData.nodes)
            {
                nodePositions.Add(node.Pos);
            }
        }

        for (int i = 0; i < 23; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                int a = 600 - 50 * i;
                int b = 330 - j * 50;

                nextPos = new Vector3(a, 0, b);

                if (nodePositions.Contains(nextPos))
                {
                    continue;
                }

                Node newnode = new Node();
                newnode.Pos = nextPos;
                script.graphData.nodes.Add(newnode);
                nodePositions.Add(nextPos);

                if (Physics.SphereCast(nextPos, 0.5f, Vector3.up, out hit, Mathf.Infinity, Wall))
                {
                    newnode.isOpen = false;
                }
            }
        }
    }

    //public string s;
    [ContextMenu("Make Path And Get Neighbor Node")] // path instantiate
    public void MakePathAndNeighborNode()
    {
        var nodes = script.graphData.nodes;
        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogError("Nodes list is null or empty.");
            return;
        }

        int width = 18;
        int height = nodes.Count / width;

        ConcurrentDictionary<Path, bool> pathHash = new ConcurrentDictionary<Path, bool>();
        ConcurrentBag<Path> newPaths = new ConcurrentBag<Path>();

        Parallel.For(0, height, y =>
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (index >= nodes.Count)
                {
                    Debug.LogError($"Index out of range: {index}");
                    continue;
                }

                Node node = nodes[index];
                if (!node.isOpen) continue;

                List<Node> nodelist = new List<Node>();
                if (x > 0) nodelist.Add(nodes[index - 1]); // 왼쪽
                if (x < width - 1) nodelist.Add(nodes[index + 1]); // 오른쪽
                if (y > 0) nodelist.Add(nodes[index - width]); // 위
                if (y < height - 1) nodelist.Add(nodes[index + width]); // 아래
                //if(index == 0)
                //s = $"{nodelist[0].NumberForNode},{nodelist[1].NumberForNode},";
                foreach (Node neighbor in nodelist)
                {
                    if (!neighbor.isOpen) continue;

                    Path newpath = new Path(node, neighbor);
                    Path reversePath = new Path(neighbor, node);
                    //s = $"{node.NumberForNode}, {neighbor.NumberForNode}";
                    if (!pathHash.ContainsKey(newpath) && !pathHash.ContainsKey(reversePath))
                    {
                        pathHash[newpath] = true;
                        //newPaths.Add(newpath);
                        script.graphData.paths.Add(newpath);
                    }
                }
            }
        });
    }
    [ContextMenu("Path Check")]
    public void PathCheck()
    {
        foreach (Path path in script.graphData.paths)
        {
            Vector3 direction = path.nodeA.Pos - path.nodeB.Pos;
            float distance = direction.magnitude;
            direction.Normalize();
            if (!Physics.Raycast(path.nodeB.Pos, direction, distance))
                path.isOpen = true;
            else
                path.isOpen = false;
        }
    }
    [ContextMenu("reset  Node")]
    public void resetNode()
    {
        script.graphData.nodes.Clear();
        script.graphData.nodeSorted.Clear();
    }
    [ContextMenu("reset  Path")]
    public void resetPath()
    {
        script.graphData.paths.Clear();
        script.graphData.PathSorted.Clear();
    }
    [ContextMenu("Get Sorted")]
    public void GetDictionary()
    {
        script.graphData.nodeSorted.Clear();
        script.graphData.PathSorted.Clear();
    }
    private void OnDrawGizmos()
    {

        if (script.graphData != null)
        {
            //Gizmos.color = Color.green;
            //Gizmos.DrawSphere(script.graphData.nodes[1].Pos, 25);


            foreach (Node node in script.graphData.nodes)
            {
                if (node.isOpen)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(node.Pos, script.graphData.nodeSize);
                }
                else
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(node.Pos, script.graphData.nodeSize);
                }
                Handles.Label(node.Pos + Vector3.up * script.graphData.nodeSize, $"Node {node.NumberForNode -1}");
            }
            foreach (Path path in script.graphData.paths)
            {
                if (path.isOpen)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(path.nodeA.Pos, path.nodeB.Pos);
                }
                else
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(path.nodeA.Pos, path.nodeB.Pos);
                }
            }
        }
    }
}
