using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;


public class Third_PersonCamera : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject targetCinemachine;
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    public float BottomClamp = 30f; // 
    public float TopClamp = 85f;

    public CharacterController controller;
    private float speed;
    private float aniBlend;
    private float targetRotation;
    private float speedChangeRate;
    public float sprintSpeed;
    public float moveSpeed;
    
    private CharacterInput inputC;
    
    
    private const float threshHold = 0.01f;
    
    public bool LockCameraPosition = false;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("Main Camera");
    }

    private void Start()
    {
        cinemachineTargetYaw = targetCinemachine.transform.rotation.eulerAngles.y;
        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void CameraRotation()
    {
        if(inputC.look.sqrMagnitude >= threshHold && LockCameraPosition)
        {
            float rotateSpeed = 1f;
            cinemachineTargetYaw += rotateSpeed * Time.deltaTime;
            cinemachineTargetPitch += rotateSpeed * Time.deltaTime;
        }
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetYaw = ClampAngle(cinemachineTargetPitch,BottomClamp,TopClamp);
    }

    private static float ClampAngle(float ifAngle ,float ifMin,float ifMax)
    {
        if (ifAngle > 360) ifAngle -= 360;
        if (ifAngle < -360) ifAngle += 360;
        return Mathf.Clamp(ifAngle,ifMin,ifMax);
    }

    private void Move()
    {
        float targetSpeed = inputC.sprint ? sprintSpeed : moveSpeed;
        if (inputC.move == Vector2.zero) targetSpeed = 0f;

        float currentHorSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        float speedOffset = .1f;
        float inputMagnitude = inputC.analogMovement ? inputC.move.magnitude : 1f;
        if(currentHorSpeed < inputMagnitude - speedOffset||
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
        if(aniBlend < 0f) aniBlend = 0f;

        Vector3 inputDir = new Vector3(inputC.move.x,0,inputC.move.y).normalized; 

        if(inputC.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDir.x,inputDir.y) * Mathf.Rad2Deg;// ¿©±îÁö
        }
    }
}
