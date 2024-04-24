using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    [Header("Slope")]
    public float slopeCheckerOffSet;
    public float maxSlopeAngle;

    [SerializeField]
    private LayerMask GroundLayer;
    [Header("Debug")]
    public bool onGizmo;

    private void OnDrawGizmos()
    {
        if(onGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * slopeCheckerOffSet);

            
        }
    }
        RaycastHit hitInfo;
    public bool SlopeCheck()
    {
        if (Physics.Raycast(transform.position,Vector3.down, out hitInfo,slopeCheckerOffSet,GroundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, hitInfo.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    public Vector3 GetSlopeMoveDirection(Vector3 moveDir)
    {
        return Vector3.ProjectOnPlane(moveDir,hitInfo.normal).normalized;
    }


}
