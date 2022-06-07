using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandManager;

public class PointAction : MonoBehaviour
{
    [SerializeField] private Transform rayPosition;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private GameObject confirmationCanvas;

    private Transform lastCollision;

    [SerializeField]
    private bool rayInteractorEnabled;

    public void ResetHoverCollision()
    {
        if (lastCollision!=null)
        {
            lastCollision.GetComponent<UIHoverButton>().TimerActive(false);
        }
    }

    /// <summary>
    /// Changes hand state from idle to point based on input.
    /// Called by PlayerController. 
    /// </summary>
    /// <param name="point"></param>
    public void PointActivation(bool point, HandType leftOrRight)
    {
        if (HandManager.Instance.GetHeldObject(leftOrRight))
        {
            return;
        }

        if (point)
        {
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.POINT, leftOrRight);
        }
        else
        {
            HandManager.Instance.ChangePose(HandPose.POINT, HandPose.IDLE, leftOrRight);
        }
    }
}
