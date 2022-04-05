using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationManager : MonoBehaviour
{

    public Animator HandAnimatorL { get; set; }
    public Animator HandAnimatorR { get; set; }

    public static HandAnimationManager Instance { get { return _instance; } }
    private static HandAnimationManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        XRCustomController.OnHandAttached += FindHand;
    }

    private void FindHand(HandType leftRight)
    {
        SwitchPose(HandPose.IDLE, leftRight);
    }

    public void ChangePose(HandPose pose, HandType leftRight)
    {
        SwitchPose(pose, leftRight);
    }

    private void SwitchPose(HandPose pose, HandType leftRight)
    {
        Animator handAnimator = leftRight.Equals(HandType.LEFT) ? HandAnimatorL : HandAnimatorR;
        switch (pose)
        {
            case HandPose.IDLE:
                handAnimator.SetTrigger("Idle");
                break;
            case HandPose.GRAB:
                handAnimator.SetTrigger("Grab");
                break;
            case HandPose.SELECT:
                handAnimator.SetTrigger("Select");
                break;
            default:
                break;
        }
    }

    public bool IsCurrentState(string stateName, HandType leftRight)
    {
        Animator handAnimator = leftRight.Equals(HandType.LEFT) ? HandAnimatorL : HandAnimatorR;
        return handAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public enum HandPose
    {
        IDLE,
        GRAB,
        SELECT
    }

    public enum HandType
    {
        LEFT,
        RIGHT
    }
}

