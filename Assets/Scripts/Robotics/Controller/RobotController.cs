using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public Transform[] bones = new Transform[8];

    public int selectedBone = 0;

    public float moveSpeed;
    public float rotateSpeed;

    private float directionModifier;

    private Vector3 axis;

    private XRCustomController controllerLeft, controllerRight;


    [field: SerializeField]
    private TextUpdater textUpdater;

    private bool pressureButtonHeld = false;

    private JoystickInteractor joystickInteractor;

    private bool axisSetOne = true;

    [HideInInspector]
    public CustomInteractor Interactor;

    private void Start()
    {
        controllerLeft = GameObject.FindGameObjectWithTag("ControllerLeft").GetComponent<XRCustomController>();
        controllerRight = GameObject.FindGameObjectWithTag("ControllerRight").GetComponent<XRCustomController>();
        controllerLeft.leftTriggerPressAction.action.performed += TriggerValue;
        controllerLeft.leftTriggerPressAction.action.canceled += TriggerValue;
        controllerRight.joystickAxisValueAction.action.performed += ThumbstickAction;
        controllerRight.changeAxisAction.action.started += ChangeAxisAction;
        joystickInteractor = controllerRight.GetComponent<JoystickInteractor>();
        Interactor = GetComponent<CustomInteractor>();
        textUpdater.UpdateText(axisSetOne ? "1  2  3" : "4  5  6");
    }

    private void ChangeAxisAction(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1)
        {
            axisSetOne = !axisSetOne;
            textUpdater.UpdateText(axisSetOne ? "1  2  3" : "4  5  6");
        }
    }

    private void TriggerValue(InputAction.CallbackContext obj)
    {
        if (Interactor.HeldObject == null)
            return;
        if (Interactor.HeldObject.transform.name == "Flexpendant")
        {
            pressureButtonHeld = obj.ReadValue<float>() == 1 ? true : false;
        }
    }

    private Vector2 joystickInput;

    private void ThumbstickAction(InputAction.CallbackContext obj)
    {
        joystickInput = obj.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        if (pressureButtonHeld)
        {
            ManualRobotControls();
        }
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

    //private void ChangeAxis()
    //{
    //    if (selectedBone == 0 || selectedBone == 1)
    //    {
    //        axis = Vector3.forward;
    //    }
    //    else if (selectedBone == 3 || selectedBone == 5)
    //    {
    //        axis = Vector3.up;
    //    }
    //    else if (selectedBone == 2 || selectedBone == 4)
    //    {
    //        axis = Vector3.right;
    //    }
    //    else
    //    {
    //        throw new ArgumentException("Selected bone case unsupported.");
    //    }
    //}
}
