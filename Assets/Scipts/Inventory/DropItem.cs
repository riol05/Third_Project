using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEditor.Progress;
using TMPro;

public class DropItem : MonoBehaviour, IInteractable
{
    public int itemID;
    [SerializeField]
    private Item item;

    [SerializeField]
    private Transform showText;
    [SerializeField]
    private TextMeshProUGUI ItemText;
    private void OnEnable()
    {
        ItemText.text = $"{item.itemName}";
        showText.gameObject.SetActive(false);
        StartCoroutine(willDestroy());
    }
    private void Update()
    {
        Interact();
    }

    IEnumerator willDestroy()
    {
        yield return new WaitForSeconds(20f);
        ObjectPoolingManager.Instance.DespawnItem(this);
    }

        RaycastHit hit;
    public void Interact()
    {
        if(Physics.SphereCast(transform.position, 3f, Vector3.zero,out hit))
        {
            if(hit.transform.GetComponent<PlayerInput>())
            {
                showText.gameObject.SetActive(true);
                showText.LookAt(hit.transform); // Player�� ���Ĵٺ��� �ٲ�����
                // TODO :
                if (CharacterInput.instance.checkDropItem) // z Ű�� �������� Input system�� bool ������ �ƴ� int�� ����ؼ� int�� �ö󰣸�ŭ ������ ȹ��
                    GetItemInventory();
            }
        }
    }

    public void GetItemInventory()
    {
        CharacterInput.instance.checkDropItem = false;
        Inventory.instance.CheckTypeForGetItem(item);
        ObjectPoolingManager.Instance.DespawnItem(this);
    }
}
