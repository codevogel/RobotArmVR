using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class XRCustomController : CustomActionBasedController
{

    #region Right Hand

    [Header("Custom hand actions - Right")]
    [Space(25)]
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
    InputActionProperty m_rightTriggerPressAction;
    public InputActionProperty rightTriggerPressAction
    {
        get => m_rightTriggerPressAction;
        set => SetInputActionProperty(ref m_rightTriggerPressAction, value);
    }

    #endregion

    #region Left Hand
    [Header("Custom hand actions - Left")]
    [Space(10)]

    [SerializeField]
    InputActionProperty m_leftTriggerPressAction;
    public InputActionProperty leftTriggerPressAction
    {
        get => m_leftTriggerPressAction;
        set => SetInputActionProperty(ref m_leftTriggerPressAction, value);
    }

    #endregion

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_thumbstickValueAction.EnableDirectAction();
        m_changeAxisAction.EnableDirectAction();
        m_leftTriggerPressAction.EnableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_thumbstickValueAction.DisableDirectAction();
        m_changeAxisAction.DisableDirectAction();
        m_leftTriggerPressAction.DisableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();

    }
}
