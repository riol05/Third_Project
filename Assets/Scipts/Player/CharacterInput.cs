using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterInput : MonoBehaviour
{
    public bool freeze;

    public bool inventoryOn;
    public bool equipmentOn;

    public static CharacterInput instance;
    public Vector2 look;
    public Vector2 move;
    public bool sprint;
    public bool jump;
    public bool wire;
    public bool analogMovement;


    public bool isChangeWeaponTime;
    public int changeWeapon;

    public bool isCursorLock = true;
    public bool cursorInputForLook = true;

    public int checkDropItem;

    public bool attack;

    private void Awake()
    {
        instance = this;
    }
    public void OnEquipment(InputValue value) => EquipInput(value.isPressed);
    private void EquipInput(bool ison) => equipmentOn = ison;
    public void OnInventory(InputValue value) => InvenInput(value.isPressed);
    private void InvenInput(bool ison) => inventoryOn = ison;

    public void OnMove(InputValue value)
    {
        if(!freeze)
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if (!freeze)
        {
            if (isCursorLock)
               LookInput(value.Get<Vector2>());
        }
    }
    public void OnSkillChange(InputValue value)
    {
        var ScrollValue = value.Get<float>();
        if (!freeze)
            ScrollInput(ScrollValue);
        ChangeTime(value.isPressed);

    }
    public void OnJump(InputValue value)
    {
        if (!freeze)
            JumpInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        if (!freeze)
            SprintInput(value.isPressed);
    }
    public void OnWire(InputValue value)
    {
        if (!freeze)
            WireInput(value.isPressed);
    }

    public void OnGetItem(InputValue Value) // inputsystem에 키 지정
    {
        if (!freeze)
            GetItemInput();
    }

    public void OnAttack(InputValue value)
    {
        if (!freeze)
            Attack(value.isPressed);
    }

    private void Attack(bool ison)
    {
        attack = ison;
    }

    private void ChangeTime(bool ison)
    {
        
    }
    private void ScrollInput(float f)
    {
        if (isChangeWeaponTime)// 이건 스킬 변경키를 어떻게 하냐에 따라 바꾸는걸로
        {
            isChangeWeaponTime = false;
            if (f < 0)
            {
                f = 0;
                ++changeWeapon;
            }
            else if (f > 0)
            {
                f = 0;
                --changeWeapon;
            }
            if (changeWeapon > 3)
                changeWeapon = 0;
            else if (changeWeapon < 0)
                changeWeapon = 2;
        }
    }
    private void SprintInput(bool ison)
    {
        sprint = ison;
    }
    private void WireInput(bool ison)
    {
        wire = ison;
    }
    private void JumpInput(bool ison)
    {
        jump = ison;
    }
    private void LookInput(Vector2 Lookdir)
    {

        look = Lookdir;
    }

    private void MoveInput(Vector2 movedir)
    {
        move = movedir;
    }

    private void GetItemInput()
    {
        ++checkDropItem;
    }
    private void OnApplicationFocus(bool focus)
    {
        SetCursorState(isCursorLock);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

