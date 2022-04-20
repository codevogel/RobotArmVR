using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandManager;

[RequireComponent(typeof(JoystickControl))]
public class JoystickInteractor : MonoBehaviour
{

    private XRCustomController rightHandController;
    private bool controllerInRange;

    private Transform originalParent;
    private Transform attachpoint;
    private Transform joystickPivot;

    private JoystickControl joystickControl;

    #region Joystick tilt
    public bool joystickPressed;
    private bool setInitialRotation;
    private Vector3 originalAttachAngle;
    private float initRotationZ;
    [SerializeField] 
    [Tooltip("Left/Right tilt allowance in degrees")]
    private float tiltAllowance;

    private float _tiltAngle;

    public float TiltAllowance { get { return tiltAllowance; } }
    

    public float TiltAngle { get { return _tiltAngle; } }

    #endregion 

    private void Start()
    {
        rightHandController = GetComponent<XRCustomController>();
        XRCustomController.OnHandAttached += FindHand;
        joystickControl = GetComponent<JoystickControl>();
    }

    public void PressJoystick(bool input, HandType leftRight)
    {
        if (HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB) && leftRight.Equals(HandType.RIGHT))
        {
            joystickPressed = input;
            if (joystickPressed)
            {
                setInitialRotation = true;
            }
            else
            {
                attachpoint.localRotation = Quaternion.Euler(originalAttachAngle);
            }
        }
    }

    public void RotateController(Quaternion input, HandType leftRight)
    {
        if (joystickPressed && HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB) && leftRight.Equals(HandType.RIGHT))
        {
            Quaternion handRotation = input;
            if (setInitialRotation)
            {
                initRotationZ = handRotation.eulerAngles.z;
                originalAttachAngle = attachpoint.localEulerAngles;
                setInitialRotation = false;
            }

            TiltHand(Mathf.DeltaAngle(initRotationZ, handRotation.eulerAngles.z));
        }
    }

    private void TiltHand(float tiltAngle)
    {
        _tiltAngle = tiltAngle;
        Vector3 newAttachAngle = originalAttachAngle;
        newAttachAngle.z += Mathf.Clamp(tiltAngle, -tiltAllowance, tiltAllowance);
        attachpoint.localRotation = Quaternion.Euler(newAttachAngle);
    }

    private void LateUpdate()
    {
        if (HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB))
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

    public void SnapToJoystick(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            return;
        }

        Transform heldObjectInLeft = HandManager.Instance.GetHeldObject(HandType.LEFT);
        if (input.Equals(true) && heldObjectInLeft != null && heldObjectInLeft.CompareTag("Flexpendant"))
        {
            rightHandController.enableInputTracking = false;
            originalParent = transform.parent;
            transform.parent = attachpoint;
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.JOYSTICK_GRAB, HandType.RIGHT);
            return;
        }
        else if (input.Equals(false) && transform.parent.name.Equals("RightHandAttach"))
        {
            transform.parent = originalParent;
            rightHandController.enableInputTracking = true;
            HandManager.Instance.ChangePose(HandPose.JOYSTICK_GRAB, HandPose.IDLE, HandType.RIGHT);
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