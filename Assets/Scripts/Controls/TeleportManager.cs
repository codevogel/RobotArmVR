using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{

    private Transform directInteractor;
    private Transform rayInteractor;

    public float joystickYTreshhold;

    public float teleportDistance;

    private bool directInteracting;

    private void Start()
    {
        directInteractor = transform.Find("DirectInteractor");
        rayInteractor = transform.Find("RayInteractor");
        rayInteractor.GetComponent<XRRayInteractor>().maxRaycastDistance = teleportDistance;
        SwitchToDirectInteraction();
    }

    public void ReadJoystickAxis(float joystickY)
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

    public void SwitchToTeleport()
    {
        directInteractor.gameObject.SetActive(false);
        rayInteractor.gameObject.SetActive(true);
        directInteracting = false;
    }

    public void SwitchToDirectInteraction()
    {
        RequestTeleport();
        directInteractor.gameObject.SetActive(true);
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
