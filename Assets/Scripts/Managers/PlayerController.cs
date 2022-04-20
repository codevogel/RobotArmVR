using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static HandManager;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private RobotController robotController;
    [SerializeField] private JoystickInteractor joystickInteractor;

    public static PlayerController Instance { get; private set; }
    public static ControllerValues Left { get; private set; }
    public static ControllerValues Right { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Left = new ControllerValues();
        Right = new ControllerValues();

        XRCustomController left = HandManager.Instance.LeftController;
        XRCustomController right = HandManager.Instance.RightController;

        SubscribeToActions(left, HandType.LEFT);
        SubscribeToActions(right, HandType.RIGHT);
    }

    private void SubscribeToActions(XRCustomController controller, HandType leftRight)
    {
        SubscribeToAction(controller.primaryButtonPressedAction.action, PrimaryButtonPressed, leftRight);
        SubscribeToAction(controller.selectAction.action, GripPressed, leftRight);
        SubscribeToAction(controller.joystickAxisValueAction.action, JoystickValue, leftRight);
        SubscribeToAction(controller.joystickPressedAction.action, JoystickPressed, leftRight);
        SubscribeToAction(controller.rotationAction.action, RotateController, leftRight);
        SubscribeToAction(controller.triggerPressAction.action, TriggerPressed, leftRight);
        SubscribeToAction(controller.joystickTouchedAction.action, JoystickTouched, leftRight);
    }

    private void JoystickTouched(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.JoystickTouched = ctx.ReadValue<float>() == 1 ? true : false;

        joystickInteractor.SnapToJoystick(controllerValues.JoystickTouched, leftRight);
    }

    private void SubscribeToAction(InputAction action, Action<InputAction.CallbackContext, HandType> callbackFunction, HandType leftRight)
    {
        action.performed += ctx => callbackFunction(ctx, leftRight);
        action.canceled += ctx => callbackFunction(ctx, leftRight);
    }

    private void RotateController(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.Rotation = ctx.ReadValue<Quaternion>();

        joystickInteractor.RotateController(controllerValues.Rotation, leftRight);
    }

    private void JoystickPressed(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.JoystickPressed = ctx.ReadValue<float>().Equals(1f) ? true : false;

        joystickInteractor.PressJoystick(controllerValues.JoystickPressed, leftRight);
    }

    private void JoystickValue(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.JoystickAxis = ctx.ReadValue<Vector2>();

        if (leftRight.Equals(HandType.LEFT))
        {
            HandManager.Instance.LeftController.teleportControls.ReadJoystickAxis(controllerValues.JoystickAxis.y);
        }
        else
        {
            robotController.ThumbstickAction(controllerValues.JoystickAxis);
        }
    }

    private void GripPressed(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.GripPressed = ctx.ReadValue<float>().Equals(1f) ? true : false;
    }

    private void PrimaryButtonPressed(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.PrimaryButtonPressed = ctx.ReadValue<float>().Equals(1f) ? true : false;

        robotController.ChangeAxisAction(controllerValues.PrimaryButtonPressed, leftRight);
    }

    private void TriggerPressed(InputAction.CallbackContext ctx, HandType leftRight)
    {
        ControllerValues controllerValues = leftRight.Equals(HandType.LEFT) ? Left : Right;
        controllerValues.TriggerPressed = ctx.ReadValue<float>().Equals(1f) ? true : false;

        bool pressed = controllerValues.TriggerPressed;

        robotController.TriggerValue(pressed, leftRight);

        XRCustomController controller = leftRight.Equals(HandType.LEFT) ? HandManager.Instance.LeftController : HandManager.Instance.RightController;
        controller.PointAction(pressed);
    }

    public struct ControllerValues
    {
        public bool TriggerPressed { get; internal set; }

        public bool GripPressed { get; internal set; }

        public Vector2 JoystickAxis { get; internal set; }

        public bool JoystickPressed { get; internal set; }

        public bool PrimaryButtonPressed { get; internal set; }

        public Quaternion Rotation { get; internal set; }
        public bool JoystickTouched { get; internal set; }

        public ControllerValues(bool triggerPressed, bool gripPressed, Vector2 joystick, bool joystickPressed, bool primaryButton, Quaternion rotation, bool joystickTouched)
        {
            TriggerPressed = triggerPressed;
            GripPressed = gripPressed;
            JoystickPressed = joystickPressed;
            JoystickAxis = joystick;
            PrimaryButtonPressed = primaryButton;
            Rotation = rotation;
            JoystickTouched = joystickTouched;
        }
    }
}
