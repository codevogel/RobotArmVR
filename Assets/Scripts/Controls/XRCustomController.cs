using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class XRCustomController : CustomActionBasedController
{
    [Header("Custom Actions")]
    [SerializeField]
    InputActionProperty m_thumbstickValueAction;
    public InputActionProperty thumbstickValueAction
    {
        get => m_thumbstickValueAction;
        set => SetInputActionProperty(ref m_thumbstickValueAction, value);
    }

    [SerializeField]
    InputActionProperty m_axisUpAction;
    public InputActionProperty axisUpAction
    {
        get => m_axisUpAction;
        set => SetInputActionProperty(ref m_axisUpAction, value);
    }

    [SerializeField]
    InputActionProperty m_axisDownAction;
    public InputActionProperty axisDownAction
    {
        get => m_axisDownAction;
        set => SetInputActionProperty(ref m_axisDownAction, value);
    }

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_thumbstickValueAction.EnableDirectAction();
        m_axisDownAction.EnableDirectAction();
        m_axisUpAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_thumbstickValueAction.DisableDirectAction();
        m_axisUpAction.DisableDirectAction();
        m_axisDownAction.DisableDirectAction();
    }
}
