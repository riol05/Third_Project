using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterInput : MonoBehaviour
{
    public bool freeze;
    
    public static CharacterInput instance;

    public bool inventoryOn;
    public bool equipmentOn;

    public Vector2 look;
    public Vector2 move;
    public bool sprint;
    public bool jump;
    public bool wire;
    public bool analogMovement;

    public bool isChangeWeaponTime;
    public int changeWeapon;
    public float changeCD;

    public bool isCursorLock = true;

    public bool checkDropItem;

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
        if (freeze) return;
        MoveInput(value.Get<Vector2>());
    }
    public void OnAttack(InputValue value)
    {
        if(freeze) return;
        Attackinput(value.isPressed);
    }
    private void Attackinput(bool ison)
    {
        attack = ison;
    }
    public void OnLook(InputValue value)
    {
        if (freeze) return;
            if (isCursorLock)
               LookInput(value.Get<Vector2>());
    }
    public void OnSkillChange(InputValue value)
    {
        var ScrollValue = value.Get<float>();
        if (freeze) return;
        ScrollInput(ScrollValue);
    }
    public void OnJump(InputValue value)
    {
        if (freeze) return;
        JumpInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        if (!freeze)
            SprintInput(value.isPressed);
    }
    public void OnWire(InputValue value)
    {
        if (freeze) return;
        WireInput(value.isPressed);
    }

    public void OnGetItem(InputValue Value) // inputsystem에 키 지정 z키
    {
        GetItemInput(Value.isPressed);
    }
    private void ScrollInput(float f)
    {
        isChangeWeaponTime = true;
        if (Time.deltaTime > changeCD + 2f)
        {
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
        else
        {
            UIManager.instance.alert.Alert("장비 교체를 위해 잠시만 기다려주세요");
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
        movedir.y = movedir.y == 0 ? 0 : (movedir.y < 0 ? -1 : 1);
        movedir.x = movedir.x == 0 ? 0 : (movedir.x < 0 ? -1 : 1);

        move = movedir;
    }

    private void GetItemInput(bool ison )
    {
        checkDropItem = ison;
    }
    //private void OnApplicationFocus(bool focus)
    //{
    //    SetCursorState(isCursorLock);
    //}

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    private void FixedUpdate()
    {
        if (freeze) isCursorLock = false;
        else isCursorLock = true;

        SetCursorState(isCursorLock);
    }
}

