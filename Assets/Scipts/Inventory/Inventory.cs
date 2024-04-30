using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]
    InventoryWindow PotionWindow;

    [SerializeField]
    InventoryWindow IngradientWindow;
    bool ison;
    public static bool inventoryActivated = false;

    private void Awake()
    {
        Instance = this;
    }
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
