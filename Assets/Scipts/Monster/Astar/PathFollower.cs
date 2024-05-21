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
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PathFollower : MonoBehaviour
{
    private float speed;
    private  bool IsOnPoint(int pointIndex) { return (transform.position - pathToFollow[pointIndex]).sqrMagnitude < 5f; }
    private bool isEndPoint(int pointIndex) { return pointIndex == EndIndex(); }
    private int EndIndex() { return pathToFollow.Count -1; } // -1
    private int GetNextIndex(int currentIndex) { int nextIndex = -1; if (currentIndex < EndIndex()) nextIndex = currentIndex + 1; return nextIndex; }


    public LayerMask groundMask;
    private List<Vector3> pathToFollow =new List<Vector3>();
    private int currentIndex;
    private Vector3 LastDir;
    public bool isCloseDir()
    {
        if (pathToFollow == null) return true;

        if (pathToFollow.Count == 0) return true;

        if (IsOnPoint(EndIndex()) ) return true;
        
        return false;
    }
        
    public void Follow(Vector3 dir, float MoveSpeed)
    {
        stopFollow();
        pathToFollow = null; // 제대로 안가면 이코드일듯
        currentIndex = 0;
        GetNodeToPositionList(dir);
        speed = MoveSpeed;
    }

    public void stopFollow() => StopAllCoroutines();
    public void FollowContinuing() => StartCoroutine(FollowPath());
    IEnumerator FollowPath()
    {
        yield return null;
        print(EndIndex());
        while (true)// true
        {
            currentIndex = Mathf.Clamp(currentIndex, 0, EndIndex());

            if (IsOnPoint(currentIndex))
            {
                if (isEndPoint(currentIndex)) break;
                currentIndex = GetNextIndex(currentIndex);
            }
            else
            {
                if (isEndPoint(currentIndex)) // 마지막노드 전에 저장해둔 Lastdir을 저장해준다
                    pathToFollow.Add(LastDir);
                MoveTo(currentIndex);
            }

            yield return null;
        }
        Debug.Log("coroutine end");
        yield return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position +Vector3.down * 0.55f);
    }
    bool isGrounded = true;
    private void MoveTo(Vector3 dir)
    {
        var deltaPos = dir - transform.position;
        Vector3 gravity = new Vector3(0, -0.3f, 0);

        if (transform.localPosition.y > 0)
        {
            //Debug.Log("isFlying");
            dir += gravity * Time.deltaTime;
        }
        transform.up = Vector3.up;
        transform.forward = deltaPos.normalized;
        //print("tra "+transform.localPosition);
        //print("dir"+dir);
        transform.position = Vector3.MoveTowards(transform.position,dir,speed * Time.deltaTime);
    }
    private void GetNodeToPositionList(Vector3 f)
    //IEnumerator GetnodeToPosition(Vector3 f)
    {
        Node fromNode = PathFinder.instance.graphData.GetNode(FindNearNode(-transform.localPosition));
        Node toNode = PathFinder.instance.graphData.GetNode(FindNearNode(f));
        print("n1"+fromNode.Pos);
        print("n2" + toNode.Pos);

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
        } // 안되면 다시 생각 해보자. 이 클래스에서 비동기 메서드를 통한 방법은??
        
        if (toNode == null)
        {
            Debug.LogError("ToNode is null");
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
            LastDir = f; // 
            FollowContinuing();

            //pathToFollow.Add(f);
        });

        Debug.Log(f);
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