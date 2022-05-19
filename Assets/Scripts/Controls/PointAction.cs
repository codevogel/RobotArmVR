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

    private Gradient pointGradient;
    private GradientColorKey[] pointColorKey;
    private GradientAlphaKey[] alphaColorKey;

    private Gradient validGradient;
    private Gradient invalidGradient;

    private void Start()
    {
        rayInteractor.transform.gameObject.SetActive(false);
        pointGradient = new Gradient();

        pointColorKey = new GradientColorKey[2];
        pointColorKey[0].color = Color.blue;
        pointColorKey[0].time = 0;
        pointColorKey[1].color = Color.blue;
        pointColorKey[1].time = 1;

        alphaColorKey = new GradientAlphaKey[2];
        alphaColorKey[0].alpha = 1;
        alphaColorKey[0].time = 0;
        alphaColorKey[1].alpha = 1;
        alphaColorKey[1].time = 0;

        pointGradient.SetKeys(pointColorKey, alphaColorKey);

        validGradient = rayInteractor.transform.GetComponent<XRInteractorLineVisual>().validColorGradient;
        invalidGradient = rayInteractor.transform.GetComponent<XRInteractorLineVisual>().invalidColorGradient;
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
        if (HandManager.Instance.GetHeldObject(leftOrRight))
        {
            return;
        }

        isPointing = point;
        if (point)
        {
            rayInteractor.transform.GetComponent<XRInteractorLineVisual>().validColorGradient = pointGradient;
            rayInteractor.transform.GetComponent<XRInteractorLineVisual>().invalidColorGradient= pointGradient;
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.POINT, leftOrRight);
            rayInteractor.gameObject.SetActive(true);
        }
        else
        {
            rayInteractor.transform.GetComponent<XRInteractorLineVisual>().validColorGradient = validGradient;
            rayInteractor.transform.GetComponent<XRInteractorLineVisual>().invalidColorGradient = invalidGradient;
            HandManager.Instance.ChangePose(HandPose.POINT, HandPose.IDLE, leftOrRight);
            rayInteractor.gameObject.SetActive(false);
        }
    }
}
