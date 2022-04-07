using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

public class JoystickControl : ActionToControl
{

    protected override void OnActionStarted(InputAction.CallbackContext ctx)
    {
        base.OnActionStarted(ctx);
        Debug.Log("started");
        Debug.Log(ctx.ReadValue<Vector2>());
    }

    protected override void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        base.OnActionPerformed(ctx);
        Debug.Log(ctx.ReadValue<Vector2>());
    }

    protected override void OnActionCanceled(InputAction.CallbackContext ctx)
    {
        base.OnActionCanceled(ctx);
    }

}
