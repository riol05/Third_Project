using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    Vector3[] elevatorPos; // ���������Ϳ� ���� Transform�� ��ġ ����
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
        // ���������� �� ����
        OpenDoor();
        someOneInElevator.Enqueue(someone);
        // ���������� ��ġ�� �ɾ��
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
