using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Reward/Item", fileName = "RewardItem_")]

public class ItemReward : Reward
{
    [SerializeField]
    Item item;

    public override void Give(Quest quest)
    {
        Inventory.instance.CheckTypeForGetItem(item);
    }
}
