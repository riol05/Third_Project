using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeForHook : MonoBehaviour
{
    private LineRenderer lineRend;
    public AnimationCurve effectOverTime;
    public AnimationCurve curve;
    public AnimationCurve curveEffectOverDistance;

    #region 줄 길이, 그래플링 시간 관련
    public int segment = 100; // 줄 길이
    private float ropeTime;
    private float animSpeed = 1.5f;
    private float curveSize = 5f;
    private float scrollSpeed = 5f;
    #endregion

    #region 로프 시작 지점, 도착 지점
    Vector3[] vectors;
    Vector3 start;
    Vector3 end;
    private float _time;

    Quaternion forward;
    Vector3 up, defaultPos;

    private float delta, realDelta, calcTime, effect;
    int i = 0, d = 0;
    #endregion

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }
    public void UpdateRope()
    {
        ProcessBounce();
    }
    private void ProcessBounce()
    {
        vectors = new Vector3[segment + 1];
        ropeTime = Mathf.MoveTowards(ropeTime, 1f, 
            Mathf.Max(Mathf.Lerp(ropeTime, 1f, animSpeed* Time.deltaTime) - ropeTime , 0.2f * Time.deltaTime));

        vectors[0] = start;
        forward = Quaternion.LookRotation(start, end);
        up = forward * Vector3.up;
        
        for(i = 1; i < segment +1; i++) 
        {
            delta = 1f / segment * i;
            realDelta = delta * curveSize;
            if(realDelta > 1f)
            {
                d = (int)(realDelta - 1);
                realDelta -= d;
                if(realDelta > 1f)
                {
                    realDelta -= 1f;
                }
                calcTime = realDelta + -scrollSpeed * _time;
                if (calcTime < 0)
                {
                    calcTime -= (int)calcTime;
                    if (calcTime < 0)
                        calcTime += 1f;
                }
                defaultPos = GetPos(delta);
                effect = Eval(effectOverTime, _time) * Eval(curveEffectOverDistance, delta) * Eval(curve, calcTime);
                vectors[i] = defaultPos + up *effect;
            }
            lineRend.positionCount = vectors.Length;
            lineRend.SetPositions(vectors);
        }
    }

    private Vector3 GetPos(float f)
    {
        return Vector3.Lerp(start, end, f);
    }
    private static float Eval(AnimationCurve ac, float f)
    {
        return ac.Evaluate(f * ac.keys.Select(k => k.time).Max());
    }

    private bool active = false;
    public void Grapple(Vector3 start, Vector3 end)
    {
        active = true;
        _time = 0f;
        this.start = start;
        this.end = end;
    }
    public void UnGrapple()
    {
        active = false;
    }
    public void UpdateStart(Vector3 start)
    {
        this.start = start;
    }
}
