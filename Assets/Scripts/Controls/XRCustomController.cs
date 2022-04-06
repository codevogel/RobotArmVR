using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static HandAnimationManager;

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

    #region Both Hands
    [Header("Custom hand actions - Both")]
    [Space(10)]
    [SerializeField]
    InputActionProperty m_customSelectAction;
    public InputActionProperty customSelectAction
    {
        get => m_customSelectAction;
        set => SetInputActionProperty(ref m_customSelectAction, value);
    }
    #endregion

    #region Hand attachement
    private bool handAttached = false;

    public HandType leftOrRight;

    public delegate void HandAttached(HandType left);

    public static event HandAttached OnHandAttached;

    private void Start()
    {
        customSelectAction.action.performed += CustomSelect;
        customSelectAction.action.canceled += CustomSelect;
    }

    protected override void UpdateController()
    {
        base.UpdateController();
        if (!handAttached)
        {
            if (leftOrRight.Equals(HandType.LEFT))
            {
                HandAnimationManager.Instance.HandAnimatorL = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
            }
            else
            {
                HandAnimationManager.Instance.HandAnimatorR = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
            }
            OnHandAttached(leftOrRight);
            handAttached = true;
        }
    }
    #endregion

    private void CustomSelect(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1f)
        {
            HandAnimationManager.Instance.ChangePose(HandPose.IDLE, HandPose.SELECT, leftOrRight);
        }
        else
        {
            HandAnimationManager.Instance.ChangePose(HandPose.SELECT, HandPose.IDLE, leftOrRight);
        }
    }

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_thumbstickValueAction.EnableDirectAction();
        m_changeAxisAction.EnableDirectAction();
        m_leftTriggerPressAction.EnableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();
        m_customSelectAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_thumbstickValueAction.DisableDirectAction();
        m_changeAxisAction.DisableDirectAction();
        m_leftTriggerPressAction.DisableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();
        m_customSelectAction.DisableDirectAction();
    }

    #region Black sorcery for resetting tracking offset when it's off
    public bool resetPosition;

    protected override void ApplyControllerState(XRInteractionUpdateOrder.UpdatePhase updatePhase, XRControllerState controllerState)
    {
        base.ApplyControllerState(updatePhase, controllerState);
        if (!enableInputTracking)
        {
            base.transform.localPosition = Vector3.zero;
            base.transform.localRotation = Quaternion.identity;
        }
    }
    #endregion
}
