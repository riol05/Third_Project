using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterInput : MonoBehaviour
{
       
    public Vector2 look;
    public Vector2 move;
    public bool sprint;
    public bool jump;
    public bool wire;

    public bool analogMovement;

    public bool isCursorLock = true;
    public bool cursorInputForLook = true;
    
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
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
        print($"{movedir}");
        move = movedir;
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

