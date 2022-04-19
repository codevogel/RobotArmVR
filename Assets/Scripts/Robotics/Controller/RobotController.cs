using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static HandManager;

public class RobotController : MonoBehaviour
{
    public Transform[] bones = new Transform[8];

    public int selectedBone = 0;

    public float moveSpeed;
    public float rotateSpeed;

    private float directionModifier;

    private Vector3 axis;

    private Vector2 joystickInput;

    [field: SerializeField]
    private TextUpdater textUpdater;

    private bool pressureButtonHeld = false;

    private JoystickInteractor joystickInteractor;

    private bool axisSetOne = true;

    [HideInInspector]
    public CustomInteractor Interactor;

    private void Start()
    {
        joystickInteractor = HandManager.Instance.RightController.GetComponent<JoystickInteractor>();
        Interactor = GetComponent<CustomInteractor>();
        textUpdater.UpdateText(axisSetOne ? "1  2  3" : "4  5  6");
    }

    private void FixedUpdate()
    {
        if (pressureButtonHeld)
        {
            ManualRobotControls();
        }
    }

    public void ChangeAxisAction(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.RIGHT))
        {
            return;
        }
        if (input.Equals(true))
        {
            axisSetOne = !axisSetOne;
            textUpdater.UpdateText(axisSetOne ? "1  2  3" : "4  5  6");
        }
    }

    public void TriggerValue(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.RIGHT))
        {
            return;
        }

        Transform heldDevice = HandManager.Instance.GetHeldObject(HandManager.HandType.LEFT);

        if (heldDevice == null)
            return;

        if (heldDevice.transform.name == "Flexpendant")
        {
            pressureButtonHeld = input;
        }
    }

    public void ThumbstickAction(Vector2 input)
    {
        joystickInput = input;
    }

    private void ManualRobotControls()
    {
        bool move = false;
        if (joystickInteractor.joystickPressed)
        {
            if (Math.Abs(joystickInteractor.TiltAngle) > joystickInteractor.TiltAllowance)
            {
                move = true;
                axis = axisSetOne ? Vector3.right : Vector3.up;
                selectedBone = axisSetOne ? 2 : 5;
                directionModifier = joystickInteractor.TiltAngle > 0 ? 1f : -1f;
            }
        }
        else if (Math.Abs(joystickInput.x) > 0.01f || Math.Abs(joystickInput.y) > 0.01f)
        {
            bool modifyingX = Math.Abs(joystickInput.x) > Math.Abs(joystickInput.y) ? true : false;

            if (modifyingX)
            {
                move = true;
                axis = axisSetOne ? Vector3.up : Vector3.up;
                selectedBone = axisSetOne ? 0 : 3;
                directionModifier = joystickInput.x > 0 ? 1f : -1f;
            }
            else
            {
                move = true;
                axis =  axisSetOne ? Vector3.forward : Vector3.right;
                selectedBone = axisSetOne ? 1 : 4;
                directionModifier = joystickInput.y > 0 ? 1f : -1f;
            }
        }
        if (move)
        {
            bones[selectedBone].Rotate(axis, rotateSpeed * directionModifier * Time.deltaTime);
        }
    }
}
