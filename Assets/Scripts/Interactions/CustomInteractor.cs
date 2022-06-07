using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandManager;

/// <summary>
/// Extends from XRDirectInteractor.
/// Used to change hand poses and store held objects.
/// </summary>
public class CustomInteractor : MonoBehaviour
{
    private HandType leftOrRight;

    private void Start()
    {
        leftOrRight = GetComponentInParent<XRCustomController>().leftOrRight;
    }

    /// <summary>
    /// Called when object is attached to this interactor.
    /// </summary>
    public void OnAttachObject(Transform attachedObject )
    {
        HandManager.Instance.SetHeldObject(leftOrRight, attachedObject);
        HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.GRAB, leftOrRight);
    }

    /// <summary>
    /// Called when object is detached from this interactor.
    /// </summary>
    public void OnDetachObject()
    {
        HandManager.Instance.SetHeldObject(leftOrRight, null);
        HandManager.Instance.ChangePose(HandPose.GRAB, HandPose.IDLE, leftOrRight);

    }
}
