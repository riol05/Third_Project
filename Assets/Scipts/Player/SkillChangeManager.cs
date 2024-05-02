using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChangeManager : MonoBehaviour
{
    private int weaponNum;

    public bool isOn;

    private List<ISkillAble> AllSkill;

    private ISkillAble curSkill;

    [SerializeField]
    private List<GameObject> quickSlot;

    [SerializeField]
    private GameObject slotParent;

    private void Awake()
    {
        
    }
    private void Update()
    {
        if (isOn)
        {
            SelectWeapon(CharacterInput.instance.changeWeapon);
        }
    }
    private void SelectWeapon(int i)
    {
        foreach(Transform weapon in transform)
        {
            if (weaponNum == i)
                UsethisWeapon(i);
                //weapon.gameObject.SetActive(false);
            ++i;
        }
    }
    private void UsethisWeapon(int i)
    {
        weaponNum = i;

    }
    private void SetSkillQuickSlot()
    {

    }
}