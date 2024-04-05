using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;


public class Third_PersonCamera : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject targetCinemachine;
    private float cinemachineTargetYaw;
    private CharacterInput input;

    public bool LockCameraPosition = false;
    private const float threshOld = 0.01f;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("Main Camera");
    }

    private void Start()
    {
        cinemachineTargetYaw = targetCinemachine.transform.rotation.eulerAngles.y;
    }

    public void CameraRotation()
    {
        if(input.look.sqrMagnitude >= threshOld && LockCameraPosition)
        {

        }
    }
}
