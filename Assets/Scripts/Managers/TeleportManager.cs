using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{

    private Transform rayInteractor;

    public float joystickYTreshhold;

    public float teleportDistance;

    private bool teleportActivated;

    private void Start()
    {
        rayInteractor = transform.Find("RayInteractor");
        rayInteractor.GetComponent<XRRayInteractor>().maxRaycastDistance = teleportDistance;
        SwitchToDirectInteraction();
    }

    /// <summary>
    /// Switches to teleport controls if joystick Y exceeds the joystickYTreshhold.
    /// </summary>
    /// <param name="joystickY">the joystick y axis value</param>
    public void SwitchToTeleport(float joystickY)
    {
        if (joystickY >= joystickYTreshhold)
        {
            if (!teleportActivated)
            {
                SwitchToTeleport();
            }
            return;
        }
        if (teleportActivated)
        {
            SwitchToDirectInteraction();
        }

    }

    /// <summary>
    /// Switch to teleport controls
    /// </summary>
    public void SwitchToTeleport()
    {
        rayInteractor.gameObject.SetActive(true);
        teleportActivated = true;
    }

    /// <summary>
    /// Turn off teleport controls
    /// </summary>
    public void SwitchToDirectInteraction()
    {
        RequestTeleport();
        rayInteractor.gameObject.SetActive(false);
        teleportActivated = false;
    }

    /// <summary>
    /// Request a teleport
    /// </summary>
    private void RequestTeleport()
    {
        // Raycast to teleport area
        RaycastHit hit;
        LayerMask teleportLayerMask = 1<<3;
        if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward.normalized, out hit, teleportDistance,teleportLayerMask))
        {
            // If raycast hit a teleport area
            CustomTeleportArea area = hit.transform.GetComponent<CustomTeleportArea>();
            if (area != null)
            {
                // Request the teleport
                area.RequestTeleport(rayInteractor.GetComponent<XRRayInteractor>(), hit);
                TrainingScriptManager.Instance.ActivateTrigger(0);
            }
        }
    }
}
