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

    public bool inventoryActivate; // 인벤토리가 켜져 있을때는 건들지 말자
    private bool whichInventoryActivated = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(CharacterInput.instance.)
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
    public void ShowWindow()
    {
        PotionWindow.slotParent.SetActive(whichInventoryActivated);
        IngradientWindow.slotParent.SetActive(!whichInventoryActivated);
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
