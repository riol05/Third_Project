using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Node(Vector3 Position) { Pos = Position; }
    public Vector3 Pos;
    public bool canGo { get { return cango; } }
    private bool cango;

    [HideInInspector]
    public float hDistance;
    [HideInInspector]
    public float pDistance;
    [HideInInspector]
    public Node previousNode;
}
public class Path
{
    [SerializeField]
    private Node startNode;
    [SerializeField]
    private Node endNode;

}
public class GraphData : MonoBehaviour
{
    public GameObject node;

    [ContextMenu("Hello")]
    public void Something()
    {
        Vector3 nextPos;
        for(int i = 0; i< 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                nextPos = new Vector3(i, 0, j);
                GameObject instance = Instantiate(node, nextPos, Quaternion.identity);
                instance.transform.SetParent(transform);
            }
        }
    }

}
