using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

public class JoystickControl : ActionToControl
{

    public Vector2 JoystickInput { get { return joystickInputVector; } }
    private Vector2 joystickInputVector;

    protected override void OnActionStarted(InputAction.CallbackContext ctx)
    {
        base.OnActionStarted(ctx);
        joystickInputVector = ctx.ReadValue<Vector2>();
    }

    protected override void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        base.OnActionPerformed(ctx);
        joystickInputVector = ctx.ReadValue<Vector2>();
    }

    protected override void OnActionCanceled(InputAction.CallbackContext ctx)
    {
        base.OnActionCanceled(ctx);
        joystickInputVector = Vector2.zero;
    }

}
