using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Potion,
    Ingredient,
    Equip
}
[CreateAssetMenu(menuName ="ScriptableObject/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    public int itemID;

    public ItemType Type;

    public string itemName;

    public string description;

    [SerializeField]
    private int useValue;

    public Sprite icon;
    public int buyPrice;
    public int sellPrice;

    public DropItem itemOnField;// 맵상에서 쓸 아이템 프리팹
    public void UseItem()
    {
        if(Type == ItemType.Equip)
        {
            UseEquip();
        }
        else if(Type == ItemType.Potion)
        {
            UsePotion();
        }
    }
    private int UsePotion() // TODO :
    {
        return useValue;
    }

    private void UseEquip()
    {
        SkillManager.instance.SetSkillQuickSlot(this);
    }
    public void SetItemID()
    {
        itemOnField.itemID = itemID;
    }
}