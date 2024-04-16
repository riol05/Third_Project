using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    private Animator ani;
    private CharacterInput inputC;
    public GameObject mainCamera;

    public GroundChecker checkGround;//isground();
    #region 속도 관련, 방향 회전 관련
    private float speed;
    private float aniBlend;
    private float targetRotation;
    private float speedChangeRate;
    public float sprintSpeed;
    public float moveSpeed;

    public float verticalVelocity;
    public float rotationVelocity;
    public float rotationSmoothTime;
    #endregion
    
    #region 점프
    private float fallTimeDelta;
    private float fallTimeOut;

    private float jumpTimeOutDelta;
    private float jumpTimeOut;

    private float jumpHeight;
    public float gravity;

    private float terminalVelocity;
    #endregion

    #region 애니메이션 string 관리
    private string animWalkString;
    private string animRunString;
    private string animJumpgString;
    private string animFreeFallString;

    #endregion

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("Main Camera");
        AnimationString();
    }
    private void Start()
    {
        inputC = GetComponent<CharacterInput>();
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
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

        // if(HasAnimation)
        ani.SetFloat(animWalkString, aniBlend);
        ani.SetFloat(animRunString, inputMagnitude);
    }
    
    private void OnJump()
    {
        if(checkGround.GroundedCheck())
        {
            fallTimeDelta = fallTimeOut;
            //if(hasAnimate)
            ani.SetBool(animJumpgString,false);
            ani.SetBool(animFreeFallString,false);
            
            if(verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }
            
            if(inputC.jump && jumpTimeOutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
                //if(hasAnimate)
                ani.SetBool(animJumpgString, true);
            }
            
            if(jumpTimeOutDelta >= 0f) jumpTimeOutDelta -= Time.deltaTime;
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
                //if(hasAnimator)
                ani.SetBool(animFreeFallString, true);
            }
            inputC.jump = false;
        }
        if(verticalVelocity < terminalVelocity) verticalVelocity -= gravity * Time.deltaTime;
    }
}
