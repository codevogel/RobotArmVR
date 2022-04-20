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
        rayInteractor.gameObject.SetActive(true);
        directInteracting = false;
    }

    public void SwitchToDirectInteraction()
    {
        RequestTeleport();
        rayInteractor.gameObject.SetActive(false);
        directInteracting = true;
    }

    private void RequestTeleport()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward.normalized, out hit, teleportDistance))
        {
            CustomTeleportArea area = hit.transform.GetComponent<CustomTeleportArea>();
            if (area != null)
            {
                area.RequestTeleport(rayInteractor.GetComponent<XRRayInteractor>(), hit);
            }
        }
    }

}
