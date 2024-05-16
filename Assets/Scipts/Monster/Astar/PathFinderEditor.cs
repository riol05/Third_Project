using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml.XPath;
using Unity.VisualScripting;
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
    [ContextMenu("Instantiate Node")]
    public void MakeNodeOnField()
    {
        Vector3 nextPos;
        for (int i = 0;i < 50; i++)
        {
            for(int j = 0;j < 68;j++)
            {
                int a = 600 - 25 * i;
                int b = 330 - j * 18;

                nextPos = new Vector3(a, 0, b);
                bool isExist = false;
                foreach (Node node in script.graphData.nodes)
                {
                    if (node.Pos == nextPos)
                    {
                        isExist = true;
                        break;
                    }
                }
                if(isExist)
                {
                    continue;
                }
                
                Node newnode = new Node();
                newnode.Pos = nextPos;
                script.graphData.nodes.Add(newnode);
                
                if (Physics.SphereCast(nextPos, 0.5f, Vector3.up, out hit, Mathf.Infinity, Wall))
                {
                    newnode.SetAsOpen(false);
                }
            }
        }
    }
    [ContextMenu("Instantiate Path")]
    public void MakePathOnField()
    {
        Node nodea;
        Node nodeb;
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 68; j++)
            {
                if (i == j) continue;
                if (script.graphData.nodes[i] == null || script.graphData.nodes[j] == null) continue;
                nodea = script.graphData.nodes[i];
                foreach (Node node in script.graphData.nodes[i].previousNode)
                {
                    nodeb = node;
                    if (!nodea.isOpen || !nodeb.isOpen) continue;

                    bool isExist = false;
                    foreach (Path path in script.graphData.paths)
                    {
                        if ((path.nodeA == nodea && path.nodeB == nodeb) || (path.nodeA == nodeb && path.nodeB == nodea))
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist) continue;

                    Vector3 direction = (nodea.Pos - nodeb.Pos).normalized;
                    float distance = Vector3.Distance(nodea.Pos, nodeb.Pos);
                    if (Physics.Raycast(nodea.Pos, direction, out hit, distance, Wall))
                    {
                        continue;
                    }

                    Path newpath = new Path(nodea, nodeb);
                    script.graphData.paths.Add(newpath);
                }
            }
        }
    }
    [ContextMenu("Get Neighbor Node")]
    public void GetNeighborNode()
    {
        foreach (Node node in script.graphData.nodes)
        {
            foreach (Node _node in script.graphData.nodes)
            {
            }
        }
    }

    [ContextMenu("reset  Node")]
    public void resetNode()
    {
        script.graphData.nodes.Clear();
    }
    [ContextMenu("reset  Path")]
    public void resetPath()
    {
        script.graphData.paths.Clear();
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
            }
            foreach (Path path in script.graphData.paths)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(script.graphData.nodes[path.nodeANum].Pos, script.graphData.nodes[path.nodeBNum].Pos);
            }
        }
    }
}
