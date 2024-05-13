using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;


    Elevator[] elevators;
    Dictionary<float, Transform> wayToPathDic;


    private void Awake()
    {
        instance = this;
    }
    private void FindPath(Monster thisObject,Transform targetObject, float height)
    {
        if (Physics.Raycast(thisObject.transform.position,Vector3.down,height))
        {

        }
        else
        {
            thisObject.transform.position = new Vector3(0 ,-0.1f ,0);
        }
        if(Mathf.Abs(targetObject.position.y - thisObject.transform.position.y) > 20)
        {
            wayToPathDic.Clear();
            foreach(var elevator in elevators)
            {
                wayToPathDic.Add(Vector3.Distance(thisObject.transform.position,elevator.transform.position),elevator.transform);
            }
            
            wayToPathDic.OrderBy(pair => pair.Key).FirstOrDefault();

        }
    }
}
