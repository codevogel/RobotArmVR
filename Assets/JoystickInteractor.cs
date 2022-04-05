using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(1)]
public class JoystickInteractor : MonoBehaviour
{

    private XRCustomController rightHandController;
    private bool controllerInRange;

    private Transform originalParent;
    private Transform attachpoint;

    private Animator handAnimator;

    private void Start()
    {
        rightHandController = GetComponent<XRCustomController>();
        rightHandController.rightTriggerPressAction.action.performed += SnapToJoystick;
        rightHandController.rightTriggerPressAction.action.canceled += SnapToJoystick;
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        //TODO: investigate why this delay is necessary
        yield return new WaitForSeconds(2);
        attachpoint = GameObject.FindGameObjectWithTag("Flexpendant").transform.Find("JoystickPivot").Find("Joystick").Find("RightHandAttach");
        handAnimator = transform.Find("[RightHand Controller] Model Parent").Find("HandPrefabRight(Clone)").GetComponent<Animator>();
    }

    private void SnapToJoystick(InputAction.CallbackContext obj)
    {
        Debug.Log(controllerInRange);

        if (obj.ReadValue<float>() == 1 && controllerInRange)
        {
            rightHandController.enableInputTracking = false;
            originalParent = transform.parent;
            transform.parent = attachpoint;
            handAnimator.SetTrigger("Grab");
            return;
        }
        rightHandController.enableInputTracking = true;
        transform.parent = originalParent;
        handAnimator.SetTrigger("Idle");
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
