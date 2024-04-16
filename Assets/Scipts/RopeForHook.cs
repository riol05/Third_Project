using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeForHook : MonoBehaviour
{
    public int segment = 100; // ¡Ÿ ±Ê¿Ã
    private LineRenderer lineRend;
    private float ropeTime;
    private float animSpeed = 1.5f;
    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    Vector3[] vectors;
    Vector3 start;
    Vector3 end;
    Quaternion forward;
    Vector3 up, defaultPos;
    public void UpdateRope()
    {
        ProcessBounce();
    }

    private void ProcessBounce()
    {
        vectors = new Vector3[segment];
        ropeTime = Mathf.MoveTowards(ropeTime, 1f, 
            Mathf.Max(Mathf.Lerp(ropeTime, 1f, animSpeed* Time.deltaTime) - ropeTime , 0.2f * Time.deltaTime));

        vectors[0] = start;
        forward = Quaternion.LookRotation(start, end);
        up = forward * Vector3.up;
        //todo;
    }

}
