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
                showText.LookAt(hit.transform); // Player를 안쳐다보면 바꿔주자
                // TODO :
                if (CharacterInput.instance.checkDropItem) // z 키에 넣을예정 Input system은 bool 변수가 아닌 int를 사용해서 int가 올라간만큼 아이템 획득
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
