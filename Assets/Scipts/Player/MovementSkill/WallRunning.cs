using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    private CharacterMovement cm;
    private ObjectChecker cheker;
    private CharacterInput inputC;

    public bool onGizmo;
    [Header("WallCheck")]
    public float WallRadius;
    public float WallOffset;
    [SerializeField]
    private LayerMask WallLayer;

    private bool wallRight;
    private bool wallLeft;

    private float wallRunningTime;
    private float wallRunningTimeCD;

    public bool isWallRunning;
    private bool exitingWall;
    private void Awake()
    {
        cm = GetComponent<CharacterMovement>();
        cheker = GetComponent<ObjectChecker>();
        inputC = GetComponent<CharacterInput>();
    }

    private void OnDrawGizmos()
    {
        if (onGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), WallRadius);
        }
    }
    private void Update()
    {
        CheckForWall();
        CheckStateOnWall();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, WallOffset, WallLayer) ;
        wallLeft = Physics.Raycast(transform.position, -transform.right, WallOffset, WallLayer);
    }

    private void CheckStateOnWall()
    {
        if (!cm.isground && inputC.jump && CheckWall())
        if ((wallRight || wallLeft) && inputC.move.x > 0 && !cm.isground)
        {
            if (!isWallRunning)
                StartWallRunning();

            if (wallRunningTime > 0)
                wallRunningTime -= Time.deltaTime;

            if(wallRunningTime <= 0 && isWallRunning)
            {
                wallRunningTime = wallRunningTimeCD;
                exitingWall = true;
            }
        }
    }
    private void StartWallRunning()
    { 
        isWallRunning = true;
        exitingWall = false;
        wallRunningTime = wallRunningTimeCD;
    }

    public bool CheckWall()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, WallRadius, WallLayer,
            QueryTriggerInteraction.Ignore);
    }
}