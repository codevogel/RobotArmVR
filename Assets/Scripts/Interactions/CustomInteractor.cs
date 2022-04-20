using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandManager;

public class CustomInteractor : XRDirectInteractor
{

    private HandType leftOrRight;

    private void Start()
    {
        leftOrRight = GetComponentInParent<XRCustomController>().leftOrRight;
    }

    public void OnAttachObject(SelectEnterEventArgs args)
    {
        HandManager.Instance.SetHeldObject(leftOrRight, args.interactableObject.transform);
        HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.GRAB, leftOrRight);
    }

    public void OnDetachObject(SelectExitEventArgs args)
    {
        HandManager.Instance.SetHeldObject(leftOrRight, null);
        HandManager.Instance.ChangePose(HandPose.GRAB, HandPose.IDLE, leftOrRight);

    }
}
