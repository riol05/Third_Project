using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    private Slot[] slots;
    public GameObject slotParent;
    private List<Item> items;


    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set { 
                slotCnt = value;
            }
    }
    public GameObject InventorybaseObject;

    public static bool inventoryActivated = false;

    private void Start()
    {
        slotCnt = 16;
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
    private void OnValidate()
    {
        
    }
    public void GetItem(Item item)
    {
        if(slots.Length >= SlotCnt)
        {
            ThrowItem(item);
            UIManager.instance.alert.Alert("�������� ��á���ϴ�.");
        }
        else
        {

        }
    }
    public void ThrowItem(Item item)
    {
        items.Remove(item);
    }
    public void OpenInventory()
    {
        InventorybaseObject.gameObject.SetActive(true);
    }
    public void CloseInventory()
    {
        InventorybaseObject.gameObject.SetActive(false);
    }
    public void AcquireItem(Item item, int Count)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].SetSlotCount(Count);
                return;
            }
        }
        for(int i = 0;i < slots.Length; i++) 
        {
            if (slots[i].item == null)
            {
                slots[i].SetSlotCount(0);
            }
        }
    }
}
