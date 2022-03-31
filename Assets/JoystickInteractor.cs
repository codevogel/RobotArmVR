using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInteractor : MonoBehaviour
{

    private XRCustomController rightHandController;
    private bool controllerInRange;

    private Transform originalParent;
    private Transform attachpoint;

    private void Start()
    {
        rightHandController = GetComponent<XRCustomController>();
        rightHandController.rightTriggerPressAction.action.performed += SnapToJoystick;
        rightHandController.rightTriggerPressAction.action.canceled += SnapToJoystick;
        attachpoint = GameObject.FindGameObjectWithTag("Flexpendant").transform.Find("RightHandAttach").Find("RightHandRevertRotation");
        Debug.Log(attachpoint);
    }

    private void SnapToJoystick(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1 && controllerInRange)
        {
            rightHandController.enableInputTracking = false;
            originalParent = transform.parent;
            transform.parent = attachpoint;
            return;
        }
        rightHandController.enableInputTracking = true;
        transform.parent = originalParent;
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
