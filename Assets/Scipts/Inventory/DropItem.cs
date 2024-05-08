using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEditor.Progress;

public class DropItem : MonoBehaviour, IInteractable
{
    public int itemID;
    [SerializeField]
    private Item item;
    private void OnEnable()
    {
        StartCoroutine(willDestroy());
    }

    IEnumerator willDestroy()
    {
        yield return new WaitForSeconds(20f);
        ObjectPoolingManager.Instance.DespawnItem(this);
    }
    //public Item GetItemInf()
    //{
    //    // ������ �Ŵ������Լ� �÷��̾�
    //    foreach (var item in ItemManager.Instance.AllItemList)
    //    {
    //        if(itemID == item.itemID)
    //        {
    //            //ItemSO = item;
    //            return item;
    //        }
    //    }
    //    return null;
    //}
        RaycastHit hit;
    public void Interact()
    {
        if(Physics.SphereCast(transform.position, 3f, Vector3.zero,out hit))
        {
            if(hit.transform.GetComponent<PlayerInput>())
            {
                // TODO :
                // ������ ���� ui ���ֱ� 
                //if(CharacterInput.instance.GetItemKey) //  e Ű�� �������� Input system�� bool ������ �ƴ� int�� ����ؼ� int�� �ö󰣸�ŭ ������ ȹ��
                //{
                //    GetItemInventory();
                //}
            }
        }
    }

    public void GetItemInventory()
    {
        //Inventory.instance.CheckTypeForGetItem(GetItemInf());
        Inventory.instance.CheckTypeForGetItem(item);
        ObjectPoolingManager.Instance.DespawnItem(this);
    }
}
