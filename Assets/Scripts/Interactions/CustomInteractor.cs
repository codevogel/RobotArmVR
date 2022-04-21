using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandManager;

/// <summary>
/// Extends from XRDirectInteractor.
/// Used to change hand poses and store held objects.
/// </summary>
public class CustomInteractor : XRDirectInteractor
{
    private HandType leftOrRight;

    private void Start()
    {
        leftOrRight = GetComponentInParent<XRCustomController>().leftOrRight;
    }

    /// <summary>
    /// Called when object is attached to this interactor.
    /// </summary>
    /// <param name="args">The SelectEnterEventArgs</param>
    public void OnAttachObject(SelectEnterEventArgs args)
    {
        HandManager.Instance.SetHeldObject(leftOrRight, args.interactableObject.transform);
        HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.GRAB, leftOrRight);
    }

    /// <summary>
    /// Called when object is detached from this interactor.
    /// </summary>
    /// <param name="args">The SelectEnterEventArgs</param>
    public void OnDetachObject(SelectExitEventArgs args)
    {
        HandManager.Instance.SetHeldObject(leftOrRight, null);
        HandManager.Instance.ChangePose(HandPose.GRAB, HandPose.IDLE, leftOrRight);

    }
}
