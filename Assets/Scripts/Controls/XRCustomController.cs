using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static HandManager;

public class XRCustomController : CustomActionBasedController
{
    #region Right Hand

    [Header("Custom hand actions - Right")]
    [Space(25)]
    [SerializeField]
    InputActionProperty m_joystickAxisValueAction;
    public InputActionProperty joystickAxisValueAction
    {
        get => m_joystickAxisValueAction;
        set => SetInputActionProperty(ref m_joystickAxisValueAction, value);
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

    [SerializeField]
    InputActionProperty m_joystickPressedAction;
    public InputActionProperty joystickPressedAction
    {
        get => m_joystickPressedAction;
        set => SetInputActionProperty(ref m_joystickPressedAction, value);
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
    InputActionProperty m_pointAction;
    public InputActionProperty pointAction
    {
        get => m_pointAction;
        set => SetInputActionProperty(ref m_pointAction, value);
    }

    [SerializeField]
    InputActionProperty m_teleportAction;
    public InputActionProperty teleportAction
    {   
        get => m_teleportAction;
        set => SetInputActionProperty(ref m_teleportAction, value);
    }

    #endregion

    #region Hand attachement
    private bool handAttached = false;
    private bool handGrabbing = false;
    private CustomInteractor customInteractor;

    public HandType leftOrRight;

    public delegate void HandAttached(HandType left);

    public static event HandAttached OnHandAttached;

    private void Start()
    {
        pointAction.action.performed += PointAction;
        pointAction.action.canceled += PointAction;
        customInteractor = GetComponent<CustomInteractor>();
    }

    protected override void UpdateController()
    {
        base.UpdateController();
        if (!handAttached)
        {
            if (leftOrRight.Equals(HandType.LEFT))
            {
                HandManager.Instance.HandAnimatorL = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
            }
            else
            {
                HandManager.Instance.HandAnimatorR = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
            }
            OnHandAttached(leftOrRight);
            handAttached = true;
        }
    }
    #endregion

    private void PointAction(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1f)
        {
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.POINT, leftOrRight);
        }
        else
        {
            HandManager.Instance.ChangePose(HandPose.POINT, HandPose.IDLE, leftOrRight);
        }
    }

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_joystickAxisValueAction.EnableDirectAction();
        m_joystickPressedAction.EnableDirectAction();
        m_changeAxisAction.EnableDirectAction();
        m_leftTriggerPressAction.EnableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();
        m_pointAction.EnableDirectAction();
        m_teleportAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_joystickAxisValueAction.DisableDirectAction();
        m_joystickPressedAction.EnableDirectAction();
        m_changeAxisAction.DisableDirectAction();
        m_leftTriggerPressAction.DisableDirectAction();
        m_rightTriggerPressAction.EnableDirectAction();
        m_pointAction.DisableDirectAction();
        m_teleportAction.DisableDirectAction();
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
