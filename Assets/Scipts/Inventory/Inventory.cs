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
    private GameObject EquipbaseObject;
    [SerializeField]
    InventoryWindow PotionWindow;
    [SerializeField]
    InventoryWindow IngradientWindow;
    [SerializeField]
    InventoryWindow EquipmentWindow;

    private bool inventoryActivate = false; // 인벤토리가 켜져 있을때는 건들지 말자
    private bool equipmentActivate = false; // 인벤토리가 켜져 있을때는 건들지 말자

    public bool Activated = false;

    private bool whichInventoryActivated = false;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if(CharacterInput.instance.inventoryOn == true ||
            CharacterInput.instance.equipmentOn == true)
            Activated = true;
        else
            Activated = false;

        InventoryON();
        EquipmentON();
    }

    private void InventoryON()
    {
        InventorybaseObject.SetActive(CharacterInput.instance.inventoryOn);
    }
    private void EquipmentON()
    {
        EquipbaseObject.SetActive(CharacterInput.instance.equipmentOn);
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
    public void ShowWindowButton()
    {
        whichInventoryActivated = !whichInventoryActivated;
        PotionWindow.Window.SetActive(whichInventoryActivated);
        IngradientWindow.Window.SetActive(!whichInventoryActivated);
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
