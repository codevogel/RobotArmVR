using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    /// <summary>
    /// Gets left hand animator
    /// </summary>
    public Animator HandAnimatorL { get; set; }
    /// <summary>
    /// Gets right hand animator
    /// </summary>
    public Animator HandAnimatorR { get; set; }

    /// <summary>
    /// Holds the currently held object for left/right
    /// </summary>
    private Transform _heldObjectLeft;
    private Transform _heldObjectRight;

    /// <summary>
    /// Gets the Left controller.
    /// </summary>
    public XRCustomController LeftController { get; private set; }

    /// <summary>
    /// Gets the Right controller.
    /// </summary>
    public XRCustomController RightController { get; private set; }

    /// <summary>
    /// Instance used for Singleton desing pattern
    /// </summary>
    public static HandManager Instance { get { return _instance; } }
    private static HandManager _instance;

    private HandPose _currentPoseLeft = HandPose.IDLE, _currentPoseRight = HandPose.IDLE;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        XRCustomController.OnHandAttached += InitHand;
    }

    /// <summary>
    /// Get the currently held object from the left or right hand.
    /// </summary>
    /// <param name="leftRight">Held in left or right hand?</param>
    /// <returns></returns>
    public Transform GetHeldObject(HandType leftRight)
    {
        return leftRight.Equals(HandType.LEFT) ? _heldObjectLeft : _heldObjectRight;
    }


    /// <summary>
    /// Set the currently held object from the left or right hand.
    /// </summary>
    /// <param name="leftRight">Held in left or right hand?</param>
    /// <returns></returns>
    public void SetHeldObject(HandType leftRight, Transform heldObject)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            _heldObjectLeft = heldObject;
            return;
        }
        _heldObjectRight = heldObject;
    }

    /// <summary>
    /// Gets the current pose
    /// </summary>
    /// <param name="leftRight">From left or right hand?</param>
    /// <returns>The current pose for the left/right hand</returns>
    public HandPose GetCurrentPose(HandType leftRight)
    {
        return leftRight == HandType.LEFT ? _currentPoseLeft : _currentPoseRight;
    }

    /// <summary>
    /// Sets the current pose
    /// </summary>
    /// <param name="leftRight">For left or right hand?</param>
    private void SetCurrentPose(HandType leftRight, HandPose pose)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            _currentPoseLeft = pose;
            return;
        }
        _currentPoseRight = pose;
    }

    /// <summary>
    /// Initialize the hand
    /// </summary>
    /// <param name="leftRight">Left or right hand?</param>
    private void InitHand(HandType leftRight)
    {
        SwitchPose(HandPose.IDLE, HandPose.IDLE, leftRight);
    }

    /// <summary>
    /// Change pose from fromPose to toPose for left/right hand.
    /// </summary>
    /// <param name="fromPose">The pose we switch FROM.</param>
    /// <param name="toPose">The pose we switch TO.</param>
    /// <param name="leftRight">Left or right hand?</param>
    public void ChangePose(HandPose fromPose, HandPose toPose, HandType leftRight)
    {
        SwitchPose(fromPose, toPose, leftRight);
    }

    /// <summary>
    /// Switches pose from fromPose to toPose for left/right hand.
    /// </summary>
    /// <param name="fromPose">The pose we switch FROM.</param>
    /// <param name="toPose">The pose we switch TO.</param>
    /// <param name="leftRight">Left or right hand?</param>
    /// <exception cref="ArgumentException">Thrown when pose is not found</exception>
    private void SwitchPose(HandPose fromPose, HandPose toPose, HandType leftRight)
    {
        Animator handAnimator = leftRight.Equals(HandType.LEFT) ? HandAnimatorL : HandAnimatorR;

        ResetTriggers(handAnimator);

        HandPose currentPose = GetCurrentPose(leftRight);
        bool canIdle = currentPose.Equals(fromPose);
        bool isIdle = currentPose.Equals(HandPose.IDLE);
        bool hasSwitched = false;

        XRCustomController currentController = leftRight.Equals(HandType.LEFT) ? LeftController : RightController;

        switch (toPose)
        {
            case HandPose.IDLE:
                if (canIdle)
                {
                    currentController.GetComponentInChildren<CapsuleCollider>().enabled = false;
                    handAnimator.SetTrigger("Idle");
                    hasSwitched = true;
                }
                break;
            case HandPose.JOYSTICK_GRAB:
                if (leftRight.Equals(HandType.RIGHT))
                {
                    handAnimator.SetTrigger("JoystickGrab");
                    hasSwitched = true;
                }
                break;
            case HandPose.GRAB:
                if (isIdle && GetHeldObject(leftRight) != null)
                {
                    handAnimator.SetTrigger("Grab");
                    hasSwitched = true;
                }
                break;
            case HandPose.POINT:
                if (isIdle)
                {
                    currentController.GetComponentInChildren<CapsuleCollider>().enabled = true;
                    handAnimator.SetTrigger("Point");
                    hasSwitched = true;
                }
                break;
            default:
                throw new ArgumentException();
        }
        if (hasSwitched)
        {
            SetCurrentPose(leftRight, toPose);
        }
    }

    /// <summary>
    /// Used to fill the LeftController / RightController properties.
    /// </summary>
    /// <param name="xrCustomController">The controller to pass on.</param>
    /// <param name="leftOrRight">Pass on to which hand?</param>
    internal void SetController(XRCustomController xrCustomController, HandType leftOrRight)
    {
        if (leftOrRight.Equals(HandType.LEFT))
        {
            LeftController = xrCustomController;
            return;
        }
        RightController = xrCustomController;
    }

    /// <summary>
    /// Resets the animation triggers.
    /// (This ensures they don't overlap).
    /// </summary>
    /// <param name="handAnimator">The animator in which to reset the triggers.</param>
    private void ResetTriggers(Animator handAnimator)
    {
        handAnimator.ResetTrigger("Idle");
        handAnimator.ResetTrigger("Grab");
        handAnimator.ResetTrigger("JoystickGrab");
        handAnimator.ResetTrigger("Point");
    }

    /// <summary>
    /// The current hand animation pose.
    /// </summary>
    public enum HandPose
    {
        IDLE,
        JOYSTICK_GRAB,
        GRAB,
        POINT
    }

    /// <summary>
    /// Left or right hand?
    /// </summary>
    public enum HandType
    {
        LEFT,
        RIGHT
    }
}

