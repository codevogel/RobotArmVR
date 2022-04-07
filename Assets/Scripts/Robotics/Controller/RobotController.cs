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

    private CustomInteractor interactor;

    [field: SerializeField]
    private TextUpdater textUpdater;

    [SerializeField]float minTreshHoldTrigger, maxTreshHoldTrigger;

    private bool pressureButtonHeld = false;

    private string SelectedBoneText { get { return (selectedBone + 1).ToString(); } }

    private JoystickInteractor joystickInteractor;

    private void Start()
    {
        controllerLeft = GameObject.FindGameObjectWithTag("ControllerLeft").GetComponent<XRCustomController>();
        controllerRight = GameObject.FindGameObjectWithTag("ControllerRight").GetComponent<XRCustomController>();
        controllerLeft.leftTriggerPressAction.action.performed += TriggerValue;
        controllerLeft.leftTriggerPressAction.action.canceled += TriggerValue;
        controllerRight.joystickAxisValueAction.action.performed += ThumbstickAction;
        controllerRight.changeAxisAction.action.started += ChangeAxisAction;
        joystickInteractor = controllerRight.GetComponent<JoystickInteractor>();
        interactor = GetComponent<CustomInteractor>();
        textUpdater.UpdateText(SelectedBoneText);
    }

    private void ChangeAxisAction(InputAction.CallbackContext obj)
    {
        //selectedBone = (selectedBone + 1) % bones.Length;
        //textUpdater.UpdateText(SelectedBoneText);
    }

    private void TriggerValue(InputAction.CallbackContext obj)
    {
        if (interactor.HeldObject == null)
            return;
        if (interactor.HeldObject.transform.name == "Flexpendant")
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
                axis = Vector3.right;
                selectedBone = 2;
                directionModifier = joystickInteractor.TiltAngle > 0 ? 1f : -1f;
            }
        }
        else if (Math.Abs(joystickInput.x) > 0.01f || Math.Abs(joystickInput.y) > 0.01f)
        {
            bool modifyingX = Math.Abs(joystickInput.x) > Math.Abs(joystickInput.y) ? true : false;

            if (modifyingX)
            {
                move = true;
                axis = Vector3.forward;
                selectedBone = 0;
                directionModifier = joystickInput.x > 0 ? 1f : -1f;
            }
            else
            {
                move = true;
                axis = Vector3.forward;
                selectedBone = 1;
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
