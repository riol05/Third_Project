using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;


public class Third_PersonCamera : MonoBehaviour
{
    public GameObject targetCinemachine;
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    public float BottomClamp = 30f; // 
    public float TopClamp = 85f;

    private CharacterInput inputC;
    
    private const float threshHold = 0.01f;
    public bool LockCameraPosition = false;

    private void Start()
    {
        inputC = GetComponent<CharacterInput>();
        cinemachineTargetYaw = targetCinemachine.transform.rotation.eulerAngles.y;
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


}
