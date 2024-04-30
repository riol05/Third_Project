using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField]
    private GameObject InventorybaseObject;
    [SerializeField]
    InventoryWindow PotionWindow;
    [SerializeField]
    InventoryWindow IngradientWindow;
    [SerializeField]
    InventoryWindow EquipmentWindow;

    bool potionOn;
    public static bool inventoryActivated = false;

    private void Awake()
    {
        instance = this;
    }
    public void CheckTypeForGetItem(Item item)
    {
        if(item.Type == ItemType.Potion)
        {
            PotionWindow.AcquireItem(item);
        }
        else if (item.Type == ItemType.Ingredient) 
        {
            IngradientWindow.AcquireItem(item);
        }
        else if(item.Type == ItemType.Equip)
        {
            EquipmentWindow.AcquireItem(item);
        }
    }

    public void showThisWindow()
    {
        potionOn = !potionOn;
        PotionWindow.gameObject.SetActive(potionOn);
        IngradientWindow.gameObject.SetActive(!potionOn);
    }

    public void OpenInventory()
    {
        InventorybaseObject.gameObject.SetActive(true);
    }
    public void CloseInventory()
    {
        InventorybaseObject.gameObject.SetActive(false);
    }
}
