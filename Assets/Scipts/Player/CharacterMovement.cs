using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainCamera;
    
    public CharacterController controller;
    private Animator ani;
    private CharacterInput inputC;
    //public GameObject mainCamera;
    public GroundChecker checkGround;//isground();

    private Rigidbody rb;

    private bool hasAni;
    #region 속도 관련, 방향 회전 관련
    private float speed;
    private float aniBlend;
    private float targetRotation;
    private float speedChangeRate = 10;
    public float sprintSpeed;
    public float moveSpeed;

    public float verticalVelocity;
    public float rotationVelocity;
    public float rotationSmoothTime;
    #endregion

    #region 점프
    private bool isGround;
    private float fallTimeDelta;
    private float fallTimeOut = 0.5f;

    private float jumpTimeOutDelta;
    private float jumpTimeOut = 0.15f;

    private float jumpHeight;
    private  float gravity =-15f;

    private float terminalVelocity = -53;
    #endregion

    #region 애니메이션 string 관리
    private string animWalkString;
    private string animRunString;
    private string animJumpgString;
    private string animFreeFallString;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasAni = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        inputC = GetComponent<CharacterInput>();
        //mainCamera = GameObject.FindGameObjectWithTag("Main Camera");
        checkGround = GetComponent<GroundChecker>();
        AnimationString();
    }
    private void Start()
    {
        jumpTimeOutDelta = jumpTimeOut;
        fallTimeDelta = fallTimeOut;
    }
    private void Update()
    {
        isGround =checkGround.GroundedCheck();
        Move();
        OnJump();
    }
    private void AnimationString()
    {
        animJumpgString = "UserJump";
        animWalkString = "UserWalk";
        animRunString = "UserRun";
        animFreeFallString = "UserFall";
    }

    private void Move()
    {

        float targetSpeed = inputC.sprint ? sprintSpeed : moveSpeed;
        if (inputC.move == Vector2.zero) targetSpeed = 0f;
        float currentHorSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        float speedOffset = .1f;
        float inputMagnitude = inputC.analogMovement ? inputC.move.magnitude : 1f;
        if (currentHorSpeed < inputMagnitude - speedOffset ||
            currentHorSpeed > inputMagnitude + speedOffset)
        {
            speed = Mathf.Lerp(currentHorSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate); // 10f = speedChangeRate;
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        aniBlend = Mathf.Lerp(aniBlend, targetSpeed, Time.deltaTime * speedChangeRate);// 10f = speedChangerate
        if (aniBlend < 0f) aniBlend = 0f;

        Vector3 inputDir = new Vector3(inputC.move.x, 0, inputC.move.y).normalized;

        if (inputC.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

        if (hasAni)
        {
            ani.SetFloat(animWalkString, aniBlend);
            ani.SetFloat(animRunString, inputMagnitude);
        }
    }
    
    private void OnJump()
    {
        if(isGround)
        {
            fallTimeDelta = fallTimeOut;
            if (hasAni)
            {
                ani.SetBool(animJumpgString, false);
                ani.SetBool(animFreeFallString, false);
            }
            
            if(verticalVelocity < 0)
                verticalVelocity = -2f;
            
            if(inputC.jump && jumpTimeOutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
                //if(hasAnimate)
                ani.SetBool(animJumpgString, true);
            }
            
            if(jumpTimeOutDelta >= 0f)
                jumpTimeOutDelta -= Time.deltaTime;
        }
        else
        {
            jumpTimeOutDelta = jumpTimeOut;

            if(fallTimeDelta >= 0)
            {
                fallTimeDelta -= Time.deltaTime;
            }
            else
            {
                if(hasAni)
                ani.SetBool(animFreeFallString, true);
            }
            inputC.jump = false;
        }
        if(verticalVelocity < terminalVelocity) verticalVelocity -= gravity * Time.deltaTime;
    }

    private bool enableMovementOnNextTouch;
    private bool activeGrapple;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        //cam.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        //cam.DoFov(85f);
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
}
