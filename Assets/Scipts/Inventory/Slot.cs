using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 orgPos;
    [SerializeField]
    private Image image;
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
                image.color = new Color(1,1,1,1);
            }
            else
            {
                image.color = new Color(1,1,1,0);
                //CountImage.SetActive(false);
            }
        }
    }
    private void Start()
    {
        orgPos = transform.position;
    }

    public void AddItem(Item item, int count)
    {
        item = Item;
        itemCount += count;
        image.sprite = Item.icon;

        CountImage.SetActive(true);
        CountText.text = itemCount.ToString();
        image.color = new Color(1,1,1,1);
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
        image.sprite = null;
        image.color = new Color(1, 1, 1, 0);//???

        CountText.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.Type == ItemType.Equip || item.Type == ItemType.Potion)
                {
                    SetSlotCount(-1);
                    item.UseItem();
                    return;
                }
                else // ±âÅ¸ÅÛ
                    return;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(item != null)
        transform.position = eventData.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
            transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = orgPos;
    }
    public void OnDrop(PointerEventData eventData)
    {
    }
}
