using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Used to customly trigger teleportation requests.
/// Extends the TeleportationArea class.
/// </summary>
public class CustomTeleportArea : TeleportationArea
{

    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        return base.GenerateTeleportRequest(interactor, raycastHit, ref teleportRequest);
    }

    public bool RequestTeleport(IXRInteractor interactor, RaycastHit raycastHit)
    {
        TeleportRequest request = new TeleportRequest();
        bool succeeded = GenerateTeleportRequest(interactor, raycastHit, ref request);
        teleportationProvider.QueueTeleportRequest(request);
        return succeeded;
    }
}
