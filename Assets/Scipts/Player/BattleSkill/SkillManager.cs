using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private Item[] slotSkill;

    public QuickSlot CurSkill;

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
        if(i == 0)
        {
            CurSkill = null;
            UIManager.instance.alert.Alert($"장비를 해제하였습니다.");
            return;
        }
        else if ( i == 1)
        {
            CurSkill = FirstSlot;
        }
        else if (i == 2)
        {
            CurSkill = SecondSlot;
        }
        else if ( i == 3)
        {
            CurSkill = ThirdSlot;
        }
        UIManager.instance.alert.Alert($"{CurSkill.item.itemName} 을 착용하였습니다.");
    }
    private void GetSlotInformation()
    {
        int i = 0;
        foreach (Item skill in slotSkill)
        {
            if (skill == null)
            {
                return;
            }
            slots[i].AddItem(skill);
            ++i;
        }
    }
    public void SetSkillQuickSlot(Item item)
    {
        if(slotSkill.Length <= 2)
        {
            slotSkill[slotSkill.Length - 1] = item;
        }
        else
        {
            for (int i = 2; i > 0; i--) // TODO : 확인 한번만.. 헷갈려 no.1
            {
                slots[i].item = slots[i - 1].item;
                
                if(i == 0) slots[i].item = item;
            }
        }
        GetSlotInformation();
    }
}