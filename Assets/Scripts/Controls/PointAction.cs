using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandManager;

public class PointAction : MonoBehaviour
{
    [SerializeField] private Transform rayPosition;
    [SerializeField] private XRRayInteractor rayInteractor;

    private bool isPointing;
    private Transform lastCollision;
    private HandType leftRight;

    private void Start()
    {
        rayInteractor.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPointing)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward.normalized, out hit, rayInteractor.GetComponent<XRRayInteractor>().maxRaycastDistance)
                && hit.transform.GetComponent<UIHoverButton>()!=null)
            {
                hit.transform.GetComponent<UIHoverButton>().TimerActive(true);
                lastCollision = hit.transform;
            }
            else if (lastCollision!=null)
            {
                lastCollision.transform.GetComponent<UIHoverButton>().TimerActive(false);
                lastCollision = null;
            }
        }
    }

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
        leftRight = leftOrRight;

        isPointing = point;
        if (point)
        {
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.POINT, leftOrRight);
            rayInteractor.gameObject.SetActive(true);
        }
        else
        {
            HandManager.Instance.ChangePose(HandPose.POINT, HandPose.IDLE, leftOrRight);
            rayInteractor.gameObject.SetActive(false);
        }
    }
}
