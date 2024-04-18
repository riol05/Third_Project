using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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

    private bool isBlocked;

    public Vector3 location;
    private float decelerateTimer = 0f;
    private float max;
    private float targetDistance;

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

    public void Grapple()
    {
        RaycastHit hit;
        if (isBlocked)
            return;
        if(Physics.Raycast(Third_PersonCamera.instance.transform.position,
            Third_PersonCamera.instance.transform.forward,out hit, maxGrappleDistance,groundMask))
        {
            location = hit.point;
        }
    }

    public void UnGrapple()
    {
        if(!isgrappling) return; 
        if(location != null)
            //Destroy(location.gameObject); // TODO : lean pool 사용
        if (decelerateTimer == 0f)
            StartCoroutine(Decelerate());
        else
            decelerateTimer = 0f;
    }

    public void UpdateGrapple()
    {
        if(location == null) return;
        targetDistance = Vector3.Distance(location, transform.position);
        rope.segment = ((int)(targetDistance / maxGrappleDistance) * segment);
    }
    private IEnumerator Decelerate()
    {
        WaitForEndOfFrame wf = new WaitForEndOfFrame();
        max = decelerateTimer * Mathf.Clamp01(targetDistance * 10f) * Mathf.Clamp01(pm.controller.velocity.magnitude * 30f);
        for(; decelerateTimer < max; decelerateTimer += Time.deltaTime)
        {
            // TODO : rb.AddForce()를 이용해야 하지만, RigidBody를 사용하지 않을거기 때문에 직접 수식 계산
            //pm.controller.Move();
            Vector3 tarGetDir = (location - transform.position).normalized;
            float Speed = 5f;

            
            yield return wf;
        }
        decelerateTimer = 0f;
    }
}
