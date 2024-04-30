using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterInput : MonoBehaviour
{
       
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
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if (isCursorLock)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    public void OnSkillChange(InputValue value)
    {
        var ScrollValue = value.Get<float>();

        ScrollInput(ScrollValue);
    }
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnWire(InputValue value)
    {
        WireInput(value.isPressed);
    }

    public void OnChangeTime(InputValue value)
    {
        ChangeTime(value.isPressed);
    } // ????

    public void OnGetItem(InputValue Value)
    {
        GetItemInput();
    }

    public void OnAttack(InputValue value)
    {
        Attack(value.isPressed);
    }

    private void Attack(bool ison)
    {
        attack = ison;
    }

    private void ChangeTime(bool ison)
    {
        isChangeWeaponTime = ison;
    }
    private void ScrollInput(float f)
    {
        if (isChangeWeaponTime)
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
            if (changeWeapon > 4)
                changeWeapon = 0;
            else if (changeWeapon < 0)
                changeWeapon = 4;
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

