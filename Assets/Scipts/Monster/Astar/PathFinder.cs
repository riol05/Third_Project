using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    private void FindPath(Monster thisObject,Transform targetObject, float height)
    {
        if (thisObject.elevatorState == ElevatorState.elevator || thisObject.stateMon == MonsterState.Idle || thisObject.stateMon == MonsterState.Attack)
        {
            return;
        }
        if (targetObject.position.y - thisObject.transform.position.y > 20)
        {
            wayToPathDic.Clear();
            foreach (var elevator in elevators)
            {
                wayToPathDic.Add(Vector3.Distance(thisObject.transform.position, elevator.StartPos.position), elevator.transform);
            }
            var theWay = wayToPathDic.OrderBy(pair => pair.Key).FirstOrDefault();
            FindPath(thisObject, theWay.Value, height);
        }
        else if (targetObject.position.y - thisObject.transform.position.y < 15)
        {
            wayToPathDic.Clear();
            foreach (var elevator in elevators)
            {
                wayToPathDic.Add(Vector3.Distance(thisObject.transform.position, elevator.EndPos.position), elevator.transform);
            }
            var theWay = wayToPathDic.OrderBy(pair => pair.Key).FirstOrDefault();
            FindPath(thisObject, theWay.Value, height);
        }
        else   return;

        if (Physics.Raycast(thisObject.transform.position,Vector3.down,height))
        {
            wayToPathDic.Clear();
            GetMinPath();
        }
        else   thisObject.transform.position = new Vector3(0 ,-0.1f * Time.deltaTime ,0);
    }

    public int FindNearNode(Vector3 dir)
    {
        float maxValue = float.MaxValue;
        Node nearNode = null;
        dir = new Vector3(dir.x,0,dir.z);

        foreach(Node node in graphData.nodeSorted.Values)
        {
            if(nearNode == null)
            {
                if(Vector3.Distance(dir,node.Pos) < maxValue)
                {
                    nearNode = node;
                    maxValue = Vector3.Distance(dir, node.Pos);
                }
            }
        }
        return nearNode != null ? nearNode.NumberForNode : -1;
    }

    private void GetMinPath()
    {

    }


}
