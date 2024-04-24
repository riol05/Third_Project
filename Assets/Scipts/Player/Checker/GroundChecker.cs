using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public float GroundedRadius = 0.2f;
    public float GroundedOffset = 0.86f;
    [SerializeField]
    private LayerMask GroundLayers;

    [Header("Debug")]
    public bool OnGizmo;

    public bool isSlope;

    private void OnDrawGizmos()
    {
        if(OnGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(new Vector3(transform.position.x,transform.position.y - GroundedOffset,transform.position.z),GroundedRadius);
        }
    }
    public bool GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }
}
