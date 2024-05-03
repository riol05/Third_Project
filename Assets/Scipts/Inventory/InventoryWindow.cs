using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryWindow : MonoBehaviour
{
    private Slot[] slots;
    public GameObject Window;
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
    private void Start()
    {
        slotCnt = 16;
        slots = slotParent.GetComponentsInChildren<Slot>();
        slotParent.SetActive(false);
    }

    public void CheckEmptySlot(Item item)
    {
        ThrowItem(item);
        UIManager.instance.alert.Alert("¾ÆÀÌÅÛÀÌ ²ËÃ¡½À´Ï´Ù.");
    }
    public void ThrowItem(Item item)
    {
        items.Remove(item);
        //item.itemOnField.transform.position; // ÇÃ·¹ÀÌ¾î À§Ä¡·Î ¶³¾îÁü
    }

    public void AcquireItem(Item _item, int Count = 1)
    {
        if (slots.Length >= slotCnt)
        {
            CheckEmptySlot(_item);
            return;
        }
        if (ItemType.Equip != _item.Type)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item == _item)
                    {
                        slots[i].SetSlotCount(Count);
                        return;
                    }
                }
            }
        }
        for(int i = 0;i < slots.Length; i++) 
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item,Count);
                return;
            }
        }
    }
}
