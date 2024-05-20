using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
    private int EndIndex() { return pathToFollow.Count - 1; } // -1
    private int GetNextIndex(int curIndex) { int NextIndex = -1 ; if (curIndex < EndIndex()) NextIndex = curIndex + 1; return NextIndex; }

    public Transform target;
    private List<Vector3> pathToFollow =new List<Vector3>();
    private int currentIndex;
    public bool isCloseDir()
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
        stopFollow();
        GetNodeToPositionList(dir);
        speed = MoveSpeed;
        currentIndex = 0;
    }

    public void stopFollow() => StopAllCoroutines();

    public void FollowContinuing() => StartCoroutine(FollowPath());
    IEnumerator FollowPath()
    {
        yield return null;
        while (!isEndPoint(currentIndex))// true
        {
            currentIndex = Mathf.Clamp(currentIndex ,0 ,EndIndex()); // +1에 대해 생각하는거로
            Debug.Log("coroutine start");
            if (IsOnPoint(currentIndex))
                GetNextIndex(currentIndex);
            else
            {
                Debug.Log("move start");
                MoveTo(currentIndex);
            }
            yield return null;
        }
        yield return null;
        Debug.Log("coroutine End");
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
    //IEnumerator GetnodeToPosition(Vector3 f)
    {
        //pathToFollow.Add(transform.position);
        Node fromNode = PathFinder.instance.graphData.GetNode(FindNearNode(transform.position));
        Node toNode = PathFinder.instance.graphData.GetNode(FindNearNode(f));
        if (!fromNode.isOpen)
        {
            foreach (Path path in PathFinder.instance.graphData.paths)
            {
                if (path.nodeA == fromNode || path.nodeB == fromNode)
                    fromNode = path.nodeA == fromNode ? path.nodeB : null;
            }
        }
        if (fromNode == null)
        {
            while (fromNode == null)
            {
                int i = fromNode.NumberForNode % 18 == 0 ? fromNode.NumberForNode - 1 : fromNode.NumberForNode + 1;
                fromNode = PathFinder.instance.graphData.GetNode(i);
            }
        } // 안되면 다시 생각 해보자. 이 클래스에서 비동기 메서드를 통한 방법 사용 가능
        
        
        if (toNode == null)
        {
            Debug.LogError("Tonode is null");
        }
        PathFinder.instance.FindShortPathToPos(fromNode, toNode, 
            (List<Node> point) =>
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
                pathToFollow.Add(node.Pos); 
            }
        });
        pathToFollow.Add(f);

        Debug.Log(pathToFollow.Count);
        FollowContinuing();
        //yield return null;
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