using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Controller_Input : MonoBehaviour
{
    private GameObject left;
    private Hand leftHand;
    private GameObject right;
    private Hand rightHand;

    public enum Dpad
    {
        Up,
        Down,
        Left,
        Right,
        Center,
        None
    }

    // Start is called before the first frame update
    void Start()
    {
        left = GameObject.FindGameObjectWithTag("LeftHand");
        leftHand = left.GetComponent<Hand>();
        right = GameObject.FindGameObjectWithTag("RightHand");
        rightHand = right.GetComponent<Hand>();
    }

    public bool IsTeleporting()
    {
        return SteamVR_Actions._default.Teleport.GetState(leftHand.handType) ||
            SteamVR_Actions._default.Teleport.GetState(rightHand.handType);
    }

    public Dpad GetLeftDpadPosition()
    {
        return GetDpadPosition(leftHand);
    }

    public Dpad GetRightDpadPosition()
    {
        return GetDpadPosition(rightHand);
    }

    private Dpad GetDpadPosition(Hand h)
    {
        if (SteamVR_Actions._default.DpadDown.GetState(h.handType))
            return Dpad.Down;

        if (SteamVR_Actions._default.DpadLeft.GetState(h.handType))
            return Dpad.Left;

        if (SteamVR_Actions._default.DpadNeutral.GetState(h.handType))
            return Dpad.Center;

        if (SteamVR_Actions._default.DpadRight.GetState(h.handType))
            return Dpad.Right;

        if (SteamVR_Actions._default.DpadUp.GetState(h.handType))
            return Dpad.Up;

        return Dpad.None;
    }

    public Vector2 GetLeftJoystickPosition()
    {
        SteamVR_Action_Vector2 joystickPosition = SteamVR_Actions._default.LeftJoystickPosition;
        return joystickPosition.GetAxis(leftHand.handType);
    }

    public Vector2 GetRightJoystickPosition()
    {
        SteamVR_Action_Vector2 joystickPosition = SteamVR_Actions._default.RightJoystickPosition;
        return joystickPosition.GetAxis(rightHand.handType);
    }

    public bool IsDrawing(string hand)
    {
        if (hand == "LeftHand")
            return SteamVR_Actions._default.Draw.GetState(leftHand.handType);

        return SteamVR_Actions._default.Draw.GetState(rightHand.handType);
    }

    public bool IsTriggerDown()
    {
        return SteamVR_Actions._default.Trigger.GetState(leftHand.handType) ||
            SteamVR_Actions._default.Trigger.GetState(rightHand.handType);
    }

    public Vector3 GetLeftControllerPosition()
    {
        return left.transform.position;
    }

    public Vector3 GetRightControllerPosition()
    {
        return right.transform.position;
    }
}
