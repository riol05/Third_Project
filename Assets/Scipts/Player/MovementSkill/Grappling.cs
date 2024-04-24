using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    public CharacterMovement pm;
    public CinemachineVirtualCamera cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance = 100;
    public float grappleDelayTime = 0.1f;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    public CharacterInput inputC;
    [Header("Cooldown")]
    public float grapplingCd = 1f;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;
    private void Update()
    {
        if (inputC.wire) StartGrapple();

        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
            if(inputC.wire)
                inputC.wire = false;
        }
    }
    
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.transform.position + cam.transform.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
