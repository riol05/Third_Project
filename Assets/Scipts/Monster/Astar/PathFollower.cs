using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PathFollower : MonoBehaviour
{
    protected float speed;
    protected Vector3 CastToVector(System.Object ob) { if (ob is Vector3 vec) return vec; Debug.Assert(false, "CastVector3 fail"); return Vector3.zero; }
    protected Node CastToNode(System.Object ob) { if (ob is Node node) return node; Debug.Assert(false, "CastNodeFail"); return null; }
    protected virtual bool isOnPoint(int PointIndex) { Debug.LogError("Override This"); return false; }
    protected bool isEndPoint(int pointIndex) { return pointIndex == EndIndex(); }
    protected int EndIndex() {return pathToFollow.Count - 1; }
    protected int GetNextIndex(int curIndex) { int NextIndex = -1 ; if (curIndex < EndIndex()) NextIndex = curIndex + 1; return NextIndex; }


    protected List<System.Object> pathToFollow;
    protected int currentIndex;
    protected bool isRotate;
    public void Follow(List<System.Object> ob, float MoveSpeed, bool isRotate)
    {
        pathToFollow = ob;
        speed = MoveSpeed;
        this.isRotate = isRotate;
        StopAllCoroutines();
        
        StartCoroutine(FollowPath());
    }
    public void Follow(List<Vector3> ob, float MoveSpeed,bool Rotate) => Follow(ob.Cast<System.Object>().ToList(),MoveSpeed,Rotate);
    public void Follow(List<Node> ob, float MoveSpeed,bool Rotate) =>  Follow(ob.Cast<System.Object>().ToList(), MoveSpeed, Rotate);

    IEnumerator FollowPath()
    {
        print("If Error, Fix this Method");
        yield return null;
        while (!isEndPoint(currentIndex))// true
        {
            Mathf.Clamp(currentIndex ,0 ,EndIndex());
            if (isOnPoint(currentIndex))
                GetNextIndex(currentIndex);
                //if(isEndPoint(currentIndex))
                //{
                //    break;
                //}
            else
                MoveTo(currentIndex);
            yield return null;
        }
    }

    public virtual void MoveTo(int i)
    {
        Debug.LogError("OverrideError");
    }
}