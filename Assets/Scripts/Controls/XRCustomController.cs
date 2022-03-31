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
    InputActionProperty m_changeAxisAction;
    public InputActionProperty changeAxisAction
    {
        get => m_changeAxisAction;
        set => SetInputActionProperty(ref m_changeAxisAction, value);
    }

    [SerializeField]
    InputActionProperty m_triggerHoldAction;
    public InputActionProperty triggerHoldAction
    {
        get => m_triggerHoldAction;
        set => SetInputActionProperty(ref m_triggerHoldAction, value);
    }

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_thumbstickValueAction.EnableDirectAction();
        m_changeAxisAction.EnableDirectAction();
        m_triggerHoldAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_thumbstickValueAction.DisableDirectAction();
        m_changeAxisAction.DisableDirectAction();
        m_triggerHoldAction.DisableDirectAction();
    }
}
