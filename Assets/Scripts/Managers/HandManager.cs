using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    public Animator HandAnimatorL { get; set; }
    public Animator HandAnimatorR { get; set; }
    
    public Transform GetHeldObject(HandType leftRight)
    {
        return leftRight.Equals(HandType.LEFT) ? _heldObjectLeft : _heldObjectRight;
    }

    public void SetHeldObject(HandType leftRight, Transform heldObject)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            _heldObjectLeft = heldObject;
            return;
        }
        _heldObjectRight = heldObject;
    }

    private Transform _heldObjectLeft;
    private Transform _heldObjectRight;

    public static HandManager Instance { get { return _instance; } }
    private static HandManager _instance;

    private HandPose _currentPoseLeft = HandPose.IDLE, _currentPoseRight = HandPose.IDLE;

    public HandPose GetCurrentPose(HandType leftRight)
    {
        return leftRight == HandType.LEFT ? _currentPoseLeft : _currentPoseRight;
    }

    private void SetCurrentPose(HandType leftRight, HandPose pose)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            _currentPoseLeft = pose;
            return;
        }
        _currentPoseRight = pose;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        XRCustomController.OnHandAttached += InitHand;
    }

    private void InitHand(HandType leftRight)
    {
        SwitchPose(HandPose.IDLE, HandPose.IDLE, leftRight);
    }

    public void ChangePose(HandPose fromPose, HandPose toPose, HandType leftRight)
    {
        SwitchPose(fromPose, toPose, leftRight);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPose">The pose you intend to move state FROM</param>
    /// <param name="toPose">The pose you intend to move state TO</param>
    /// <param name="leftRight"></param>
    /// <exception cref="ArgumentException"></exception>
    private void SwitchPose(HandPose fromPose, HandPose toPose, HandType leftRight)
    {
        Animator handAnimator = leftRight.Equals(HandType.LEFT) ? HandAnimatorL : HandAnimatorR;

        ResetTriggers(handAnimator);

        HandPose currentPose = GetCurrentPose(leftRight);
        bool canIdle = currentPose.Equals(fromPose);
        bool isIdle = currentPose.Equals(HandPose.IDLE);
        bool hasSwitched = false;
        
        switch (toPose)
        {
            case HandPose.IDLE:
                if (canIdle)
                {
                    handAnimator.SetTrigger("Idle");
                    hasSwitched = true;
                }
                break;
            case HandPose.JOYSTICK_GRAB:
                if (isIdle)
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

    private void ResetTriggers(Animator handAnimator)
    {
        handAnimator.ResetTrigger("Idle");
        handAnimator.ResetTrigger("Grab");
        handAnimator.ResetTrigger("JoystickGrab");
        handAnimator.ResetTrigger("Point");
    }

    public bool IsCurrentState(string stateName, HandType leftRight)
    {
        Animator handAnimator = leftRight.Equals(HandType.LEFT) ? HandAnimatorL : HandAnimatorR;
        return handAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public enum HandPose
    {
        IDLE,
        JOYSTICK_GRAB,
        GRAB,
        POINT
    }

    public enum HandType
    {
        LEFT,
        RIGHT
    }
}

