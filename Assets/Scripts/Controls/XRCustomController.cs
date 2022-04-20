using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static HandManager;

public class XRCustomController : CustomActionBasedController
{
    #region Both Hands
    [Header("Custom hand actions")]
    [Space(10)]

    [SerializeField]
    InputActionProperty m_joystickAxisValueAction;
    public InputActionProperty joystickAxisValueAction
    {
        get => m_joystickAxisValueAction;
        set => SetInputActionProperty(ref m_joystickAxisValueAction, value);
    }

    [SerializeField]
    InputActionProperty m_joystickPressedAction;
    public InputActionProperty joystickPressedAction
    {
        get => m_joystickPressedAction;
        set => SetInputActionProperty(ref m_joystickPressedAction, value);
    }

    [SerializeField]
    InputActionProperty m_primaryButtonPressedAction;
    public InputActionProperty primaryButtonPressedAction
    {
        get => m_primaryButtonPressedAction;
        set => SetInputActionProperty(ref m_primaryButtonPressedAction, value);
    }

    [SerializeField]
    InputActionProperty m_teleportAction;
    public InputActionProperty teleportAction
    {   
        get => m_teleportAction;
        set => SetInputActionProperty(ref m_teleportAction, value);
    }

    [SerializeField]
    InputActionProperty m_TriggerPressAction;
    public InputActionProperty triggerPressAction
    {
        get => m_TriggerPressAction;
        set => SetInputActionProperty(ref m_TriggerPressAction, value);
    }
    #endregion

    #region Hand attachement
    private bool handAttached = false;
    private bool handGrabbing = false;
    private CustomInteractor customInteractor;

    public HandType leftOrRight;

    public delegate void HandAttached(HandType left);

    public static event HandAttached OnHandAttached;

    private TeleportManager _teleportControls;
    public TeleportManager teleportControls { get { return _teleportControls; } }


    private void Start()
    {
        customInteractor = GetComponent<CustomInteractor>();
        HandManager.Instance.SetController(this, leftOrRight);
        _teleportControls = GetComponent<TeleportManager>();
    }

    protected override void UpdateController()
    {
        base.UpdateController();
        if (!handAttached)
        {
            if (leftOrRight.Equals(HandType.LEFT))
            {
                HandManager.Instance.HandAnimatorL = transform.Find("[LeftHand Controller] Model Parent").GetChild(0).GetComponent<Animator>();
            }
            else
            {
                HandManager.Instance.HandAnimatorR = transform.Find("[RightHand Controller] Model Parent").GetChild(0).GetComponent<Animator>();
            }
            OnHandAttached(leftOrRight);
            handAttached = true;
        }
    }
    #endregion

    public void PointAction(bool input)
    {
        if (input)
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
        m_primaryButtonPressedAction.EnableDirectAction();
        m_TriggerPressAction.EnableDirectAction();
        m_teleportAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_joystickAxisValueAction.DisableDirectAction();
        m_joystickPressedAction.EnableDirectAction();
        m_primaryButtonPressedAction.DisableDirectAction();
        m_TriggerPressAction.EnableDirectAction();
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
