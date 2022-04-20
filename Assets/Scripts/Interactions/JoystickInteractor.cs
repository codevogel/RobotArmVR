using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandManager;

/// <summary>
/// Handles all joystick related interaction for the flexpendant.
/// </summary>
[RequireComponent(typeof(JoystickControl))]
public class JoystickInteractor : MonoBehaviour
{

    private XRCustomController rightHandController;


    #region Hand attachment
    private Transform originalParent;
    private Transform attachpoint;
    private Transform joystickPivot;
    #endregion


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
        XRCustomController.OnHandAttached += FindAttachPoints;
    }

    private void LateUpdate()
    {
        // Pivot joystick in direction based on joystick input
        if (HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB))
        {
            //Vector2 input = joystickControl.JoystickInput * -1;
            //TODO check if this also works:
            Vector2 input = PlayerController.Right.JoystickAxis * -1;

            joystickPivot.transform.localRotation = Quaternion.Euler(0, input.x * 30, input.y * 30);
        }
    }

    /// <summary>
    /// Called when the joystick is pressed (clicked) or released
    /// </summary>
    /// <param name="input">whether the joystick is pressed or released</param>
    /// <param name="leftRight">Whether the left or right hand triggered this call</param>
    public void PressJoystick(bool input, HandType leftRight)
    {
        if (HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB) && leftRight.Equals(HandType.RIGHT))
        {
            joystickPressed = input;
            if (joystickPressed)
            {
                // Ensure we set initial rotation in RotateController
                setInitialRotation = true;
            }
            else
            {
                // Reset hand tilt
                attachpoint.localRotation = Quaternion.Euler(originalAttachAngle);
            }
        }
    }

    /// <summary>
    /// Attempts to rotate the controller based on current controller rotation
    /// </summary>
    /// <param name="input">The current controller rotation</param>
    /// <param name="leftRight">Whether the left or right hand triggered this call</param>
    public void RotateController(Quaternion input, HandType leftRight)
    {
        // If right joystick is pressed and hand is snapped to joystick
        if (joystickPressed && leftRight.Equals(HandType.RIGHT) && HandManager.Instance.GetCurrentPose(HandType.RIGHT).Equals(HandPose.JOYSTICK_GRAB))
        {
            Quaternion handRotation = input;
            // Set the initial rotation when you start tilting the hand
            if (setInitialRotation)
            {
                initRotationZ = handRotation.eulerAngles.z;
                originalAttachAngle = attachpoint.localEulerAngles;
                setInitialRotation = false;
            }

            // Tilt the hand by the rotation
            TiltHand(Mathf.DeltaAngle(initRotationZ, handRotation.eulerAngles.z));
        }
    }

    /// <summary>
    /// Tilts the hand by tiltAngle
    /// </summary>
    /// <param name="tiltAngle">the amount (in degrees) to tilt the hand</param>
    private void TiltHand(float tiltAngle)
    {
        _tiltAngle = tiltAngle;
        Vector3 newAttachAngle = originalAttachAngle;
        newAttachAngle.z += Mathf.Clamp(tiltAngle, -tiltAllowance, tiltAllowance);
        attachpoint.localRotation = Quaternion.Euler(newAttachAngle);
    }


    /// <summary>
    /// Finds attach points for the hand. Triggered when hand is attached to controller.
    /// </summary>
    private void FindAttachPoints(HandType leftRight)
    {
        joystickPivot = GameObject.FindGameObjectWithTag("Flexpendant").transform.Find("JoystickPivot");
        attachpoint = joystickPivot.Find("Joystick").Find("RightHandAttach");
    }

    /// <summary>
    /// Snaps the hand to the joystick
    /// </summary>
    /// <param name="input">Whether we have to snap to the joystick</param>
    /// <param name="leftRight">the hand that triggered the joystick touch</param>
    public void SnapToJoystick(bool input, HandType leftRight)
    {
        // Do nothing on left hand
        if (leftRight.Equals(HandType.LEFT))
        {
            return;
        }

        // If touching joystick and left hand is holding the flexpendant
        Transform heldObjectInLeft = HandManager.Instance.GetHeldObject(HandType.LEFT);
        if (input.Equals(true) && heldObjectInLeft != null && heldObjectInLeft.CompareTag("Flexpendant"))
        {
            // Snap to joystick
            rightHandController.enableInputTracking = false;
            originalParent = transform.parent;
            transform.parent = attachpoint;
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.JOYSTICK_GRAB, HandType.RIGHT);
            return;
        }
        else if (input.Equals(false) && transform.parent.name.Equals("RightHandAttach"))
        {
            // Free hand from joystick
            transform.parent = originalParent;
            rightHandController.enableInputTracking = true;
            HandManager.Instance.ChangePose(HandPose.JOYSTICK_GRAB, HandPose.IDLE, HandType.RIGHT);
        }
    }
}