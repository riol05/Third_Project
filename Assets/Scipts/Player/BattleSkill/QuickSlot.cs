using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    private Vector3 orgPos;
    [SerializeField]
    private Image image;
    private Item Item;


    public Item item
    {
        get { return Item; }
        set
        {
            Item = value;
            if (item != null)
            {
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
                //CountImage.SetActive(false);
            }
        }
    }

    private void Start()
    {
        orgPos = transform.position;
    }

    public void AddItem(Item item)
    {
        item = Item;
        image.sprite = Item.icon;
        image.color = new Color(1, 1, 1, 1);
    }
    private void ClearSlot()
    {
        Item = null;
        image.sprite = null;
        image.color = new Color(1, 1, 1, 0);//???
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.Type == ItemType.Equip)
                {
                    Inventory.instance.CheckTypeForGetItem(item);
                    ClearSlot();
                    return;
                }
            }
        }
    }
}
