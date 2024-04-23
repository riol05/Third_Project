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
    private ObjectChecker checkObject; // isSlope();

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
        if (checkObject.CheckFront()) return;

        if (activeGrapple) return; // grapple 관련

        float targetSpeed = inputC.sprint ? sprintSpeed : moveSpeed;
        if (inputC.move == Vector2.zero) targetSpeed = 0f;
        
        float currentHorSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        float speedOffset = .1f;
        float inputMagnitude = inputC.analogMovement ? inputC.move.magnitude : 1f;

        if (currentHorSpeed < inputMagnitude - speedOffset)
        {
            speed = Mathf.Lerp(currentHorSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate); 
            speed = Mathf.Round(speed * 1000f) / 100f;

        }
        else if (currentHorSpeed > inputMagnitude + speedOffset)
        {
            speed = targetSpeed;
        }
        else
        {
            speed = targetSpeed;
        }


        aniBlend = Mathf.Lerp(aniBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (aniBlend < 0f) aniBlend = 0f;

        Vector3 inputDir = new Vector3(inputC.move.x, 0, inputC.move.y).normalized;

        targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + Third_PersonCamera.instance.vc.transform.eulerAngles.y;
        if (inputC.move != Vector2.zero)
        {
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
        if (isGround)
            rb.AddForce(targetDirection * speed * 2, ForceMode.Force);
        
        else if (checkObject.SlopeCheck() && isGround)
            rb.AddForce(checkObject.GetSlopeMoveDirection(targetDirection) * speed * 1.5f, ForceMode.Force);

        else if(!isGround)
            rb.AddForce(targetDirection * speed * 1.7f
                /*+ new Vector3(0f , verticalVelocity ,0f)*/, ForceMode.Force);
        else if (inputC.move == Vector2.zero || checkObject.CheckFront()) 
            rb.velocity = Vector3.zero;


        print(speed);
        if (hasAni)
        {
            ani.SetFloat(animWalkString, aniBlend);
            ani.SetFloat(animRunString, inputMagnitude);
        }
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
                ani.SetBool(animJumpString, true);
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
                if(hasAni)
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
