using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    Vector3[] elevatorPos; // 엘레베이터에 들어온 Transform의 위치 지정
    Queue<Transform> someOneInElevator = new Queue<Transform>();
    
    [SerializeField]
    public float OpenDoorCount;
    [SerializeField]
    private float ElevatorSpeed;

    public bool isOpen;
    public Transform StartPos;
    public Transform EndPos;
    public void CallElevator(Transform someone)
    {
        // 엘레베이터 문 열림
        OpenDoor();
        someOneInElevator.Enqueue(someone);
        // 엘레베이터 위치에 걸어가기
        CloseDoor();
        StartCoroutine(GoUpStair());
    }
    private void OpenDoor()
    {
        isOpen = true;
    }
    private void CloseDoor()
    {
        isOpen = false;
    }

    IEnumerator GoUpStair()
    {
        yield return new WaitForSeconds(OpenDoorCount);

        while (transform.position != EndPos.position)
        {
            transform.Translate(EndPos.position * Time.deltaTime * ElevatorSpeed);

        }
    }
}
