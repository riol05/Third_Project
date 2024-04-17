using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private float GroundedRadius = 0.28f;
    private float GroundedOffset = 0.86f;
    [SerializeField]
    private LayerMask GroundLayers;

    [Header("Debug")]
    public bool OnGizmo;

    private void Update()
    {
        if(GroundedCheck())
        {
            print("collision now");
        }
    }
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
