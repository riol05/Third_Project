using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    InventoryWindow PotionWindow;

    [SerializeField]
    InventoryWindow IngradientWindow;
    bool ison;
    public static bool inventoryActivated = false;
    public void CheckTypeForGetItem(Item item)
    {
        if(item.Type == ItemType.Potion)
        {
            PotionWindow.GetItem(item);
        }
        else if (item.Type == ItemType.Ingredient) 
        {
            IngradientWindow.GetItem(item);
        }
    }

    public void showThisWindow()
    {
        PotionWindow.gameObject.SetActive(ison);
        IngradientWindow.gameObject.SetActive(!ison);
    }
}
