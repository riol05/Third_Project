using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class Third_PersonCamera : MonoBehaviour
{
    public static Third_PersonCamera instance;
    public GameObject targetCinemachine;
    public CinemachineVirtualCamera vc;
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float BottomClamp = -50f; // 
    private float TopClamp = 50f;

    private CharacterInput inputC;
    
    private const float threshHold = 0.01f;
    public bool LockCameraPosition = false;
    public float CameraAngleOverride;
    public float rotateSpeed = 1f;

    private void Awake()
    {
        instance = this;
    }
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
            cinemachineTargetYaw += inputC.look.x * rotateSpeed ;
            cinemachineTargetPitch += inputC.look.y * rotateSpeed;
        }
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch,BottomClamp,TopClamp);


        targetCinemachine.transform.rotation = Quaternion.Euler(-(cinemachineTargetPitch + CameraAngleOverride), cinemachineTargetYaw, 0f);
    }

    private static float ClampAngle(float ifAngle ,float ifMin,float ifMax)
    {
        if (ifAngle > 360) ifAngle -= 360;
        if (ifAngle < -360) ifAngle += 360;
        return Mathf.Clamp(ifAngle,ifMin,ifMax);
    }
    float originalF;
    public void DoFov(float endValue)
    {
        originalF = vc.m_Lens.FieldOfView;

        vc.m_Lens.FieldOfView = endValue;
        //Invoke(nameof(ChangeFOV),0.2f);
    }

    private void ChangeFOV()
    {
        vc.m_Lens.FieldOfView = originalF;
    }
}
