using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Potion,
    Ingredient,
}
[CreateAssetMenu(menuName ="ScriptableObject/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    public ItemType Type;

    public string name;

    public string description;

    public Sprite icon;
    public int Price;
}