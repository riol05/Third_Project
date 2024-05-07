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

    private void OnEnable()
    {
        StartCoroutine(willDestroy());
    }

    IEnumerator willDestroy()
    {
        yield return new WaitForSeconds(20f);
        this.gameObject.SetActive(false); // TODO : 시간이 지나면 오브젝트 풀링
    }
    public Item GetItemInf()
    {
        // 아이템 매니저에게서 플레이어
        foreach (var item in ItemManager.Instance.AllItemList)
        {
            if(itemID == item.itemID)
            {
                //ItemSO = item;
                return item;
            }
        }
        return null;
    }
        RaycastHit hit;
    public void Interact()
    {
        if(Physics.SphereCast(transform.position, 3f, Vector3.zero,out hit))
        {
            if(hit.transform.GetComponent<PlayerInput>())
            {
                // TODO :
                // 아이템 정보 ui 켜주기 
                //if(CharacterInput.instance.GetItemKey) //  e 키에 넣을예정 Input system은 bool 변수가 아닌 int를 사용해서 int가 올라간만큼 아이템 획득
                //{
                //    GetItemInventory();
                //}
            }
        }
    }

    public void GetItemInventory()
    {
        Inventory.instance.CheckTypeForGetItem(GetItemInf());
        Destroy(this.gameObject); // TODO : 오브젝트 풀링 사용
    }
}
