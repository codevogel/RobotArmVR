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

    private bool thumbstickPressed;

    [SerializeField] 
    [Tooltip("Left/Right tilt allowance in degrees")]
    private float tiltAllowance;

    private void Start()
    {
        rightHandController = GetComponent<XRCustomController>();
        rightHandController.rightTriggerPressAction.action.performed += SnapToJoystick;
        rightHandController.rightTriggerPressAction.action.canceled += SnapToJoystick;
        rightHandController.rotationAction.action.performed += RotateController;
        rightHandController.joystickPressedAction.action.performed += PressJoystick;
        rightHandController.joystickPressedAction.action.canceled += PressJoystick;
        XRCustomController.OnHandAttached += FindHand;
        joystickControl = GetComponent<JoystickControl>();
    }

    private void PressJoystick(InputAction.CallbackContext obj)
    {
        thumbstickPressed = obj.ReadValue<float>() == 1;
    }

    private void RotateController(InputAction.CallbackContext obj)
    {
        if (thumbstickPressed && HandAnimationManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.GRAB))
        {
            Vector3 handRotation = obj.ReadValue<Quaternion>().eulerAngles;
            switch (GetTilt(handRotation.z))
            {
                case TiltDirection.LEFT:
                    Debug.Log("LEFT");
                    break;
                case TiltDirection.RIGHT:
                    Debug.Log("RIGHT");
                    break;
                default:
                    break;
            }

        }
    }

    private TiltDirection GetTilt(float z)
    {
        if (z < 360 - tiltAllowance && z > 180)
        {
            return TiltDirection.RIGHT;
        }
        if (z > tiltAllowance && z < 180)
            return TiltDirection.LEFT;
        return TiltDirection.NONE;
    }

    private void LateUpdate()
    {
        if (HandAnimationManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.GRAB))
        {
            Vector2 input = joystickControl.JoystickInput * -1;
            joystickPivot.transform.localRotation = Quaternion.Euler(0, input.x * 30, input.y * 30);
        }
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
            HandAnimationManager.Instance.ChangePose(HandPose.IDLE, HandPose.GRAB, HandType.RIGHT);
            return;
        }
        if (transform.parent.name.Equals("RightHandAttach"))
        {
            transform.parent = originalParent;
            rightHandController.enableInputTracking = true;
            HandAnimationManager.Instance.ChangePose(HandPose.GRAB, HandPose.IDLE, HandType.RIGHT);
        }

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

enum TiltDirection
{
    LEFT,
    RIGHT,
    NONE
}