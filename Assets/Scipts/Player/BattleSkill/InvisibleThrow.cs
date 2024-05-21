using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InvisibleThrow : MonoBehaviour
{
    Rigidbody rb;
    public bool Boom;
    public Transform rightHandPivot;
    public Vector3 target;
    private float jumpPower;
    public float powerSpeed;
    public float maxJumpPower = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rightHandPivot = transform.parent;
        ThrowCalc();
    }
    void ThrowCalc()
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 startVelocity = powerSpeed * forward;
        //Vector3 startPosition = position + forward * 5 + transform.right / 2;
        //startPosition.y += 1.5f;

        rb.AddForce(startVelocity,ForceMode.Impulse);
        StartCoroutine(ThrowRoutine());
    }
    private void CommingBack()
    {
        Boom = false;
        rb.velocity = Vector3.zero;
        transform.position = rightHandPivot.position;
    }

    IEnumerator ThrowRoutine()
    {
        bool ison = true;
        while(ison)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1))
            {
                ison = false;
                yield return new WaitForSeconds(2);
            }
            yield return null;
        }
        Boom = true; // boom이 true일때 폭발하게
        yield return new WaitForSeconds(0.5f);
        CommingBack();
        yield return null;
    }
}
