using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
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
        //        CountImage.SetActive(false);
            }
        }
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
        {
            ClearSlot();
        }
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
}
