using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static HandManager;

/// <summary>
/// Stores every custom input action property
/// </summary>
public class XRCustomController : CustomActionBasedController
{
    #region Input Actions
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
    InputActionProperty m_TriggerPressAction;
    public InputActionProperty triggerPressAction
    {
        get => m_TriggerPressAction;
        set => SetInputActionProperty(ref m_TriggerPressAction, value);
    }

    [SerializeField]
    InputActionProperty m_joystickTouchedAction;
    public InputActionProperty joystickTouchedAction
    {
        get => m_joystickTouchedAction;
        set => SetInputActionProperty(ref m_joystickTouchedAction, value);
    }
    #endregion

    #region Hand attachment
    private bool handAttached = false;

    public HandType leftOrRight;

    public delegate void HandAttached(HandType left);

    /// <summary>
    /// This event is triggered when the hand has been attached to the XRCustomController.
    /// </summary>
    public static event HandAttached OnHandAttached;
    #endregion

    #region Teleportation
    private TeleportManager _teleportControls;
    public TeleportManager teleportControls { get { return _teleportControls; } }
    #endregion

    private void Start()
    {
        HandManager.Instance.SetController(this, leftOrRight);
        _teleportControls = GetComponent<TeleportManager>();
    }

    /// <summary>
    /// Changes hand state from idle to point based on input.
    /// Called by PlayerController. 
    /// </summary>
    /// <param name="point"></param>
    public void PointAction(bool point)
    {
        if (point)
        {
            HandManager.Instance.ChangePose(HandPose.IDLE, HandPose.POINT, leftOrRight);
        }
        else
        {
            HandManager.Instance.ChangePose(HandPose.POINT, HandPose.IDLE, leftOrRight);
        }
    }

    #region Package overrides

    protected override void CustomEnableAllDirectActions()
    {
        base.CustomEnableAllDirectActions();
        m_joystickAxisValueAction.EnableDirectAction();
        m_joystickPressedAction.EnableDirectAction();
        m_primaryButtonPressedAction.EnableDirectAction();
        m_TriggerPressAction.EnableDirectAction();
        m_joystickTouchedAction.EnableDirectAction();
    }

    protected override void CustomDisableAllDirectActions()
    {
        base.CustomDisableAllDirectActions();
        m_joystickAxisValueAction.DisableDirectAction();
        m_joystickPressedAction.EnableDirectAction();
        m_primaryButtonPressedAction.DisableDirectAction();
        m_TriggerPressAction.EnableDirectAction();
        m_joystickTouchedAction.DisableDirectAction();
    }

    /// <summary>
    /// Called every frame when the controllers are spawned in.
    /// Updates the references in HandManager on first call.
    /// </summary>
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

    #region Black sorcery for resetting tracking offset when it's off
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

    #endregion
}
