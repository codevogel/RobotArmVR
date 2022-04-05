using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandAnimationManager;

[RequireComponent(typeof(JoystickControl))]
public class JoystickInteractor : MonoBehaviour
{

    private XRCustomController rightHandController;
    private bool controllerInRange;

    private Transform originalParent;
    private Transform attachpoint;
    private Transform joystickPivot;

    private Animator handAnimator;

    private JoystickControl joystickControl;

    private void Start()
    {
        rightHandController = GetComponent<XRCustomController>();
        rightHandController.rightTriggerPressAction.action.performed += SnapToJoystick;
        rightHandController.rightTriggerPressAction.action.canceled += SnapToJoystick;
        XRCustomController.OnHandAttached += FindHand;
        joystickControl = GetComponent<JoystickControl>();
    }

    private void LateUpdate()
    {
        Vector2 input = joystickControl.JoystickInput * - 1;
        joystickPivot.transform.localRotation = Quaternion.Euler(0, input.x * 30, input.y * 30);
    }

    private void FindHand(HandType leftRight)
    {
        joystickPivot = GameObject.FindGameObjectWithTag("Flexpendant").transform.Find("JoystickPivot");
        attachpoint = joystickPivot.Find("Joystick").Find("RightHandAttach");
    }

    private void SnapToJoystick(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1 && controllerInRange)
        {
            rightHandController.enableInputTracking = false;
            originalParent = transform.parent;
            transform.parent = attachpoint;
            HandAnimationManager.Instance.HandAnimatorR.SetTrigger("Grab");
            return;
        }
        rightHandController.enableInputTracking = true;
        transform.parent = originalParent;
        HandAnimationManager.Instance.HandAnimatorR.SetTrigger("Idle");
    }

    public void OnJoystickEnter(OnTriggerDelegation triggerDelegation)
    {
        if (triggerDelegation.Other.CompareTag("ControllerRight"))
        {
            controllerInRange = true;
        }
    }

    public void OnJoystickExit(OnTriggerDelegation triggerDelegation)
    {
        if (triggerDelegation.Other.CompareTag("ControllerRight"))
        {
            controllerInRange = false;
        }
    }
}
