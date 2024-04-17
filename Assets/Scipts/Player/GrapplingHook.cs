using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public bool isGrappling => isgrappling;
    private bool isgrappling;
    private CharacterInput inputC;
    private CharacterMovement pm;
    
    public RopeForHook rope;

    public RectTransform crossHairSpinning;
    private int segment;

    private void Awake()
    {
        inputC = GetComponent<CharacterInput>();
        segment = rope.segment;
        pm = GetComponent<CharacterMovement>();
    }
    private int maxGrappleDistance;
    public LayerMask groundMask;
    
    private void Update()
    {
        RaycastHit hit;
        if(crossHairSpinning != null)
        {
            if(Physics.Raycast(Third_PersonCamera.instance.transform.position, Third_PersonCamera.instance.transform.forward,
                out hit, maxGrappleDistance,groundMask))
            {
                crossHairSpinning.gameObject.SetActive(true);
                crossHairSpinning.Rotate(Vector3.forward, 1f * Time.deltaTime); // 1f는 돌아가는 속도
            }
            else crossHairSpinning.gameObject.SetActive(false);
        }
        if(!isgrappling)
        {
            if(inputC.wire)
            {
                Grapple();
            }
            return;
        }
        else
        {
            if(inputC.wire)
            {
                UnGrapple();
                UpdateGrapple();
            }
            return;
        }
    }
    private bool isBlocked;
        
    public void Grapple()
    {
        GameObject rocation;
        RaycastHit hit;
        if (isBlocked)
            return;
        if(Physics.Raycast(Third_PersonCamera.instance.transform.position, Third_PersonCamera.instance.transform.forward,out hit, maxGrappleDistance,groundMask))
        {
            rocation = new GameObject();
            rocation.transform.position = hit.point;
        }
    }

    public void UnGrapple()
    {

    }

    public void UpdateGrapple()
    {

    }
}
