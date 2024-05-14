using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour // TODO : ������ �׽�Ʈ �غ���
{
    Vector3[] elevatorPos; // ���������Ϳ� ���� Transform�� ��ġ ����
    List<Transform> elevatorLifter;
    
    [SerializeField]
    public float OpenDoorCount;
    [SerializeField]
    private float ElevatorSpeed;

    public bool isOpen;
    public Transform StartPos;
    public Transform EndPos;
    public void CallElevator()
    {
        OpenDoor();
        // ���������� �� ����
        // ���������� ��ġ�� �ɾ��
        if(Physics.BoxCast(transform.position,transform.lossyScale,Vector3.forward))
        {
            StartCoroutine(GoStair());
        }
    }
    private void OpenDoor()
    {
        isOpen = true;
    }
    private void CloseDoor()
    {
        isOpen = false;
    }
    IEnumerator elevatorCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GoStair());
    }

    IEnumerator GoStair()
    {
        yield return new WaitForSeconds(OpenDoorCount);
        CloseDoor();
        Vector3 dirPos;
        if(transform.position == EndPos.position)
        {
            dirPos = StartPos.position;
        }
        else
        {
            dirPos = EndPos.position;
        }
        while (transform.position != dirPos)
        {
            transform.Translate(dirPos* Time.deltaTime * ElevatorSpeed);
        }
    }
}
