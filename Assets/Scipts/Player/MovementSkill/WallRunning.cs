using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WallRunning : MonoBehaviour
{
    private CharacterMovement cm;
    private ObjectChecker cheker;
    private CharacterInput inputC;

    public bool onGizmo;
    [Header("WallCheck")]
    public float WallRadius;
    public float wallRayDistance;
    [SerializeField]
    private LayerMask WallLayer;

    private RaycastHit rightHit;
    private RaycastHit leftHit;
    private bool wallRight;
    private bool wallLeft;
    public float wallRunningSpeed;
    public float jumpPower;
    public float wallSideForce;

    private float wallRunningTime;
    private float wallRunningTimeCD = 3f;

    private float exitWallTimer;
    private float exitWallTimerCD = 3f;

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
        CheckForWallRun();
        CheckStateOnWall();
        if(isWallRunning)
        {
            print("running now");
        }
        else
        {
            print("stop now");
        }
    }
    private void FixedUpdate()
    {
        if (isWallRunning)
            WallRunningMove();
    }
    private void CheckForWallRun()
    {
        wallRight = Physics.Raycast(transform.position, transform.right,out rightHit, wallRayDistance, WallLayer) ;
        wallLeft = Physics.Raycast(transform.position, -transform.right,out leftHit, wallRayDistance, WallLayer);
    }

    private void CheckStateOnWall()
    {
        if (!cm.isground)
        {
            if ((wallRight || wallLeft) && inputC.move.y > 0)
            {
                if (!isWallRunning)
                    StartWallRunning();

                if (wallRunningTime > 0)
                    wallRunningTime -= Time.deltaTime;

                if (wallRunningTime <= 0 && isWallRunning)
                {
                    wallRunningTime = wallRunningTimeCD;
                    exitingWall = true;
                }
                if (inputC.jump)
                    WallJump();
            }
        }
        if (exitingWall)
        {
            if (isWallRunning)
                StopwallRunning();
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0 && isWallRunning)
                exitWallTimer = exitWallTimerCD;
        }

    }
    private void WallJump()
    {
        isWallRunning = false;
        exitingWall = true;
        exitWallTimer = exitWallTimerCD;
        Vector3 wallNormal = wallRight ? rightHit.normal : leftHit.normal;
        Vector3 WallJumpForce = transform.up * jumpPower + wallNormal * wallSideForce;
        cm.rb.velocity = new Vector3(cm.rb.velocity.x,0, cm.rb.velocity.z);
       // cm.rb.AddForce(WallJumpForce,ForceMode.Force);
    }
    private void StartWallRunning()
    { 
        isWallRunning = true;
        exitingWall = false;
        wallRunningTime = wallRunningTimeCD;
        cm.rb.velocity = new Vector3 (cm.rb.velocity.x,0,cm.rb.velocity.z);
        Third_PersonCamera.instance.DoFov(40);
        //if (wallLeft) Third_PersonCamera.instance.targetCinemachine.transform.DOLocalRotate(new Vector3(0,0, -5), 0.5f);
        //if (wallRight) Third_PersonCamera.instance.targetCinemachine.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.5f);
        // TODO : 애니메이션이 불안정하면 doTween 사용
    }

    private void WallRunningMove()
    {
        Vector3 wallNormal = wallRight? rightHit.normal : leftHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal,transform.up);
        if ((transform.forward - wallForward).magnitude > (transform.forward + wallForward).magnitude)
            wallForward = -wallForward;
        
        cm.rb.AddForce(wallForward * wallRunningSpeed, ForceMode.Force);

        if (!(wallRight && inputC.move.x < 0) && !(wallLeft && inputC.move.x > 0))
            cm.rb.AddForce(-wallNormal * 100 ,ForceMode.Force);
    }
    private void StopwallRunning()
    {
        isWallRunning = false;
        exitingWall = true;
    }

    public bool CheckWall()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, WallRadius, WallLayer,
            QueryTriggerInteraction.Ignore);
    }
}