using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
        CharacterInput.instance.freeze = Activated; // 
        if (inventoryActivate == true ||
            equipmentActivate == true)
            Activated = true;
        else if (inventoryActivate == false &&
            equipmentActivate == false)
            Activated = false;

        if (CharacterInput.instance.inventoryOn) InventoryON();

        if(CharacterInput.instance.equipmentOn) EquipmentON();
    }

    private void InventoryON() 
    {
        inventoryActivate = !inventoryActivate;
        CharacterInput.instance.inventoryOn = false;
        InventorybaseObject.SetActive(inventoryActivate); 
    }
    private void EquipmentON() 
    {
        equipmentActivate = !equipmentActivate;
        CharacterInput.instance.equipmentOn = false;
        EquipbaseObject.SetActive(equipmentActivate);
    }

    public void CheckTypeForGetItem(Item item)
    {
        UIManager.instance.Item.ShowInfo(item.itemName); // 

        if(item.Type == ItemType.Potion)  PotionWindow.AcquireItem(item);
        
        else if (item.Type == ItemType.Ingredient)  IngradientWindow.AcquireItem(item);
        
        else if(item.Type == ItemType.Equip)   EquipmentWindow.AcquireItem(item);
    }
    public void ShowWindowButton()
    {
        whichInventoryActivated = !whichInventoryActivated;
        PotionWindow.Window.SetActive(whichInventoryActivated);
        IngradientWindow.Window.SetActive(!whichInventoryActivated);
    }
}
