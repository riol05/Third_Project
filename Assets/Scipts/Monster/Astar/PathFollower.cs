using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PathFollower : MonoBehaviour
{
    private float speed;
    //private Vector3 CastToVector(System.Object ob) { if (ob is Vector3 vec) return vec; Debug.Assert(false, "CastVector3 fail"); return Vector3.zero; }
    //private Node CastToNode(System.Object ob) { if (ob is Node node) return node; Debug.Assert(false, "CastNodeFail"); return null; }
    private  bool IsOnPoint(int pointIndex) { return (transform.position - pathToFollow[pointIndex]).sqrMagnitude < 0.1f; }
    private bool isEndPoint(int pointIndex) { return pointIndex == EndIndex(); }
    private int EndIndex() { ;return pathToFollow.Count - 1; }
    private int GetNextIndex(int curIndex) { int NextIndex = -1 ; if (curIndex < EndIndex()) NextIndex = curIndex + 1; return NextIndex; }


    private List<Vector3> pathToFollow =new List<Vector3>();
    private int currentIndex;
    public bool isClose()
    {
        if (pathToFollow.Count == 0)
        {
            
            return true;
        }

        if (IsOnPoint(EndIndex()) ) return true;
        
        return false;
    }
        
    public void Follow(Vector3 dir, float MoveSpeed)
    {
        GetNodeToPositionList(dir);
        speed = MoveSpeed;
        currentIndex = 0;
        stopFollow();
        StartCoroutine(FollowPath());
    }
    public void stopFollow() => StopAllCoroutines();

    IEnumerator FollowPath()
    {
        print("If Error, Fix this Method");
        yield return null;
        while (!isEndPoint(currentIndex))// true
        {
            currentIndex = Mathf.Clamp(currentIndex ,0 ,EndIndex());
            if(isEndPoint(currentIndex))
            {
                break;
            }
            if (IsOnPoint(currentIndex))
                GetNextIndex(currentIndex);
            else
                MoveTo(currentIndex);
            yield return null;
        }
    }

    private void MoveTo(Vector3 dir)
    {
        var deltaPos = dir - transform.position;
        Vector3 downForce = new Vector3(0, -1f,0);

        transform.up = Vector3.up;
        transform.forward = deltaPos.normalized;

        transform.position = Vector3.MoveTowards(transform.position,dir,speed * Time.deltaTime);
    }
    private void GetNodeToPositionList(Vector3 f)
    {
        Node nearNode = PathFinder.instance.graphData.GetNode(FindNearNode(f));
        Node nearByNode = PathFinder.instance.graphData.GetNode(FindNearNode(transform.position));

        if (nearNode == null)
        {
            Debug.LogError("node is null");
        }
        PathFinder.instance.FindShortPathToPos(nearByNode, nearNode, 
            delegate (List<Node> point)
        {
            if (point == null)
            {
                Debug.LogError("Pathfinding failed, received null path");
                return;
            }
            foreach (Node node in point)
            {
                if (node == null)
                {
                    Debug.LogError("Found null node in the path");
                    continue;
                }

                if (node == null)
                {
                    Debug.Log(node.Pos);
                    pathToFollow.Add(node.Pos);
                }
            }
            pathToFollow.Add(f);
        });
    }
    private void MoveTo(int i) => MoveTo(pathToFollow[i]);// node.numForNode로 접근할때
    public int FindNearNode(Vector3 dir)
    {
        float minDistance = float.MaxValue;
        Node nearNode = null;
        dir = new Vector3(dir.x, 0, dir.z);

        foreach (Node node in PathFinder.instance.graphData.nodeSorted.Values)
        {
            float distance = Vector3.Distance(dir, node.Pos);
            if (distance < minDistance)
            {
                nearNode = node;
                minDistance = Vector3.Distance(dir, node.Pos);
            }
        }
        return nearNode != null ? nearNode.NumberForNode : -1;
    }
}