using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{

    private CustomInteractor directInteractor;
    private Transform rayInteractor;

    public float joystickYTreshhold;

    public float teleportDistance;

    private bool directInteracting;

    private void Start()
    {
        directInteractor = GetComponent<CustomInteractor>();
        rayInteractor = transform.Find("RayInteractor");
        rayInteractor.GetComponent<XRRayInteractor>().maxRaycastDistance = teleportDistance;
        SwitchToDirectInteraction();
    }

    public void ReadJoystickAxis(float joystickY)
    {
        if (HandManager.Instance.GetHeldObject(HandManager.HandType.LEFT) == null)
        {
            if (joystickY >= joystickYTreshhold)
            {
                if (directInteracting)
                {
                    SwitchToTeleport();
                }
                return;
            }
            if (!directInteracting)
            {
                SwitchToDirectInteraction();
            }
        }

    }

    public void SwitchToTeleport()
    {
        directInteractor.enabled = false;
        rayInteractor.gameObject.SetActive(true);
        directInteracting = false;
    }

    public void SwitchToDirectInteraction()
    {
        RequestTeleport();
        directInteractor.enabled = true;
        rayInteractor.gameObject.SetActive(false);
        directInteracting = true;
    }

    private void RequestTeleport()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward.normalized, out hit, teleportDistance, LayerMask.GetMask("Teleport")))
        {
            hit.transform.GetComponent<CustomTeleportArea>().RequestTeleport(rayInteractor.GetComponent<XRRayInteractor>(), hit);
        }
    }

}
