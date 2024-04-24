using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public enum StateP
{
    Freeze,
    Move,
    Air

}
public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainCamera;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public GroundChecker checkGround;//isground();
    [HideInInspector]
    public ObjectChecker checkObject; // isSlope();
    [HideInInspector]
    public WallRunning wall;

    private Animator ani;
    private CharacterInput inputC;

    private Rigidbody rb;

    #region 속도 관련, 방향 회전 관련
    private float speed;
    private float aniBlend;
    private float targetRotation;
    private float speedChangeRate = 10;
    private float sprintSpeed;
    public float moveSpeed;

    public float rotationVelocity;
    public float rotationSmoothTime;

    private bool isSlope;
    private bool isWallHere;
    #endregion

    #region 점프
    private bool isGround;
    public bool isground { get { return isGround; } }

    private float fallTimeDelta;
    private float fallTimeOut = 0.5f;

    private float jumpTimeOutDelta;
    private float jumpTimeOut = 0.15f;

    private float jumpHeight = 2f;
    private  float gravity = -15f;

    #endregion

    #region 애니메이션 string 관리
    private bool hasAni;
    private string animWalkString;
    private string animRunString;
    private string animJumpString;
    private string animFreeFallString;
    private string animWireActionString;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasAni = GetComponent<Animator>();
        //controller = GetComponent<CharacterController>();
        inputC = GetComponent<CharacterInput>();
        checkGround = GetComponent<GroundChecker>();
        checkObject = GetComponent<ObjectChecker>();
        wall = GetComponent<WallRunning>();
        AnimationString();
    }

    private void Start()
    {
        jumpTimeOutDelta = jumpTimeOut;
        fallTimeDelta = fallTimeOut;
        sprintSpeed = moveSpeed * 2f; // 달리기 속도 설정
        Physics.gravity = Physics.gravity * 2f;
    }
    private void Update()
    {
        isGround =checkGround.GroundedCheck();
        Move();
        OnJump();
        if(freeze)
        {
            speed = 0f;
        }
        if (isGround && !activeGrapple)
        {
            rb.drag = 2;
        }
        else
            rb.drag = 0f;
    }
    private void AnimationString()
    {
        animWalkString = "Walk";
        animRunString = "Run";
        animJumpString = "Jump";
        animFreeFallString = "Fall";
        animWireActionString = "Grappling";
    }

    private void Move()
    {
        if (activeGrapple) return; // grapple 관련

        float targetSpeed = inputC.sprint ? sprintSpeed : moveSpeed;
        if (inputC.move == Vector2.zero) targetSpeed = 0f;
        float currentHorSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        float inputMagnitude = inputC.analogMovement ? inputC.move.magnitude : 1f;
        float speedOffset = .1f;
        
        if (currentHorSpeed < inputMagnitude - speedOffset)
        {
            speed = Mathf.Lerp(currentHorSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate); 
            speed = Mathf.Round(speed * 1000f) / 100f;
        }
        else
        {
            speed = targetSpeed;
        }

        Vector3 inputDir = new Vector3(inputC.move.x, 0, inputC.move.y).normalized;
        targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + Third_PersonCamera.instance.vc.transform.eulerAngles.y;
        if (inputC.move != Vector2.zero)
        {
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

        aniBlend = Mathf.Lerp(aniBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (aniBlend < 0f) aniBlend = 0f; // 애니메이션

        if (hasAni)
        {
            ani.SetFloat(animWalkString, aniBlend);
            ani.SetFloat(animRunString, inputMagnitude);
        }
        // 경사 체크, 속도 관련
        if (isGround && inputC.move == Vector2.zero || wall.CheckWall())
        {
            print("stop");
            rb.velocity = Vector3.zero;
        }
        else if (isGround || checkObject.SlopeCheck())
        {
            float accelSpeed = isGround ? 2f : 1.7f; // 지면 또는 공중에서의 가속을 구분하여 설정
            if (checkObject.SlopeCheck()) accelSpeed = 1.5f; // 경사면에서의 가속 조정

            rb.AddForce(targetDirection * speed * accelSpeed, ForceMode.Force);
            //rb.MovePosition(targetDirection * speed * accelSpeed);
        }
        else if(!isGround)
        {
            Vector3 horizontalDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            rb.AddForce(horizontalDirection * speed * 1.7f, ForceMode.Force);
        }
        print(speed);
    }
    
    private void OnJump()
    {
        if (activeGrapple) return; // grapple 관련

        if (isGround)
        {
            fallTimeDelta = fallTimeOut;
            if (hasAni)
            {
                ani.SetBool(animJumpString, false);
                ani.SetBool(animFreeFallString, false);
            }
            
            if(inputC.jump && jumpTimeOutDelta <= 0f)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                if(hasAni)
                ani.SetBool(animJumpString, true); // 점프 애니메이션
            }
            if (jumpTimeOutDelta >= 0f)
            {
                jumpTimeOutDelta -= Time.deltaTime;
                inputC.jump = false;
            }
        }
        else
        {
            jumpTimeOutDelta = jumpTimeOut;
            if(fallTimeDelta >= 0)
                fallTimeDelta -= Time.deltaTime;
            else
            {
                if(hasAni) // 추락 애니메이션
                ani.SetBool(animFreeFallString, true);
            }
        }
    }

    private bool enableMovementOnNextTouch;
    public bool activeGrapple;
    private Vector3 velocityToSet;
    public Grappling grap;

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        Invoke(nameof(ResetRestrictions), 3f);
    }

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
        Third_PersonCamera.instance.DoFov(40f);
    }

    public bool freeze;
    public void ResetRestrictions()
    {
        activeGrapple = false;
        Third_PersonCamera.instance.DoFov(85f);
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
        }
        grap.StopGrapple();
    }
}
