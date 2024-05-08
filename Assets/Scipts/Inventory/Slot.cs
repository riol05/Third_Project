using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image itemImage;
    private Item Item;
    public int itemCount;

    [SerializeField]
    TextMeshProUGUI CountText;
    [SerializeField]
    private GameObject CountImage;

    public Item item { get { return Item; }
        set
        {
            Item= value;
            if(item != null )
            {
                itemImage.color = new Color(1,1,1,1);
            }
            else
            {
                itemImage.color = new Color(1,1,1,0);
                //CountImage.SetActive(false);
            }
        }
    }

    public void AddItem(Item item, int count)
    {
        item = Item;
        itemCount += count;
        itemImage.sprite = Item.icon;

        CountImage.SetActive(true);
        CountText.text = itemCount.ToString();
        itemImage.color = new Color(1,1,1,1);
    }
    public void SetSlotCount(int count)
    {
        itemCount += count;
        CountText.text = itemCount.ToString();

        if(itemCount <=0)
            ClearSlot();
    }
    private void ClearSlot()
    {
        Item = null;
        itemCount = 0;
        itemImage.sprite = null;
        itemImage.color = new Color(1, 1, 1, 0);//???

        CountText.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.Type != ItemType.Ingredient) // 재료템이 아니면
                {
                    if(item.Type == ItemType.Potion) SetSlotCount(-1);
                    item.UseItem();
                    return;
                }
                else // 기타템
                    return;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(item != null)
            DragSlot.instance.transform.position = eventData.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragslot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragslot = null;
        //DragSlot.instance.transform.position = orgPos;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragslot != null)
            ChangeSlot();
    }

    public void ChangeSlot()
    {
        Item tempItem = item;
        int tempCount = itemCount;

        AddItem(DragSlot.instance.dragslot.item, DragSlot.instance.dragslot.itemCount);
        if(tempItem != null)
            DragSlot.instance.dragslot.AddItem(tempItem, tempCount);
        else
            DragSlot.instance.dragslot.ClearSlot();
    }
}
