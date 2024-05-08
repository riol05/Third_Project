using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private Item[] slotSkill;

    public Item CurSkill;

    [SerializeField]
    private QuickSlot FirstSlot;
    [SerializeField]
    private QuickSlot SecondSlot;
    [SerializeField]
    private QuickSlot ThirdSlot;

    [SerializeField]
    private QuickSlot[] slots;
    [SerializeField]
    private GameObject slotParent;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        slots[0] = FirstSlot;
        slots[1] = SecondSlot;
        slots[2] = ThirdSlot;
    }
    private void Update()
    {
        if (CharacterInput.instance.isChangeWeaponTime)
        {
            CharacterInput.instance.isChangeWeaponTime = false;
            UsethisWeapon(CharacterInput.instance.changeWeapon);
        }
    }
    private void UsethisWeapon(int i)
    {
        if (i == 0)
        {
            CurSkill = null;
            UIManager.instance.alert.Alert($"��� �����Ͽ����ϴ�.");
            return;
        }
        else if (i == 1)
        {
            if (FirstSlot.item == null) return;
            CurSkill = FirstSlot.item;
        }
        else if (i == 2)
        {
            if (SecondSlot.item == null) return;
            CurSkill = SecondSlot.item;
        }
        else if (i == 3)
        {
            if (ThirdSlot.item == null) return;
            CurSkill = ThirdSlot.item;
        }
        UIManager.instance.alert.Alert($"{CurSkill.itemName} �� �����Ͽ����ϴ�.");
    }
    private void GetSlotInformation() // ��ų�� ���� �����ϱ� ���Կ�
    {
        int i = 0;
        foreach (QuickSlot slot in slots)
        {
            if (slot.item == null)  return;
            slotSkill[i] = slot.item;
            ++i;
        }
    }
    public void SetSkillQuickSlot(Item item)
    {
        if (slots[slots.Length].item != null)
        {
            Inventory.instance.CheckTypeForGetItem(slots[slots.Length].item);
            for (int i = 2; i > 0; i--)
            {
                if (i == 0) slots[i].AddItem(item);
                else        slots[i].AddItem(slots[i - 1].item);
            }
        }
        else
        {
            foreach (QuickSlot slot in slots)
            {
                if (slot == null)
                {
                    slots[slots.Length].AddItem(item);
                    return;
                }
            }
        }
    }
}