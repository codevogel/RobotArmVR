using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

/// <summary>
/// Custom class with reimplemtations needed to override methods 
/// from the package that were marked as non-virtual.
/// </summary>
public class CustomActionBasedController : ActionBasedController
{
    protected virtual void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying)
            property.DisableDirectAction();

        property = value;

        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }

    protected virtual void CustomEnableAllDirectActions()
    {
        positionAction.EnableDirectAction();
        rotationAction.EnableDirectAction();
        trackingStateAction.EnableDirectAction();
        selectAction.EnableDirectAction();
        selectActionValue.EnableDirectAction();
        activateAction.EnableDirectAction();
        activateActionValue.EnableDirectAction();
        uiPressAction.EnableDirectAction();
        uiPressActionValue.EnableDirectAction();
        hapticDeviceAction.EnableDirectAction();
        rotateAnchorAction.EnableDirectAction();
        translateAnchorAction.EnableDirectAction();
    }

    protected virtual void CustomDisableAllDirectActions()
    {
        positionAction.DisableDirectAction();
        rotationAction.DisableDirectAction();
        trackingStateAction.DisableDirectAction();
        selectAction.DisableDirectAction();
        selectActionValue.DisableDirectAction();
        activateAction.DisableDirectAction();
        activateActionValue.DisableDirectAction();
        uiPressAction.DisableDirectAction();
        uiPressActionValue.DisableDirectAction();
        hapticDeviceAction.DisableDirectAction();
        rotateAnchorAction.DisableDirectAction();
        translateAnchorAction.DisableDirectAction();
    }
}

