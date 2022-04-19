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
    [SerializeField] private XRCustomController xrCustomController;
    [SerializeField] private TeleportControls teleportController;

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
        //Remove after test
        /*controller.primaryButtonPressedAction.action.performed += ctx => PrimaryButtonPressed(ctx, leftRight);
        controller.selectAction.action.performed += ctx => GripPressed(ctx, leftRight);
        controller.joystickAxisValueAction.action.performed += ctx => JoystickValue(ctx, leftRight);
        controller.joystickPressedAction.action.performed += ctx => JoystickPressed(ctx, leftRight);
        controller.rotationAction.action.performed += ctx => RotateController(ctx, leftRight);
        controller.triggerPressAction.action.performed += ctx => TriggerPressed(ctx, leftRight);
        controller.primaryButtonPressedAction.action.canceled += ctx => PrimaryButtonPressed(ctx, leftRight);
        controller.selectAction.action.canceled += ctx => GripPressed(ctx, leftRight);
        controller.joystickAxisValueAction.action.canceled += ctx => JoystickValue(ctx, leftRight);
        controller.joystickPressedAction.action.canceled += ctx => JoystickPressed(ctx, leftRight);
        controller.rotationAction.action.canceled += ctx => RotateController(ctx, leftRight);
        controller.triggerPressAction.action.canceled += ctx => TriggerPressed(ctx, leftRight);*/

        SubscribeToAction(controller.primaryButtonPressedAction.action, PrimaryButtonPressed, leftRight);
        SubscribeToAction(controller.selectAction.action, GripPressed, leftRight);
        SubscribeToAction(controller.joystickAxisValueAction.action, JoystickValue, leftRight);
        SubscribeToAction(controller.joystickPressedAction.action, JoystickPressed, leftRight);
        SubscribeToAction(controller.rotationAction.action, RotateController, leftRight);
        SubscribeToAction(controller.triggerPressAction.action, TriggerPressed, leftRight);
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
            teleportController.OnTeleportButton(controllerValues.JoystickAxis.y);
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

        joystickInteractor.SnapToJoystick(pressed, leftRight);
        robotController.TriggerValue(pressed, leftRight);
        xrCustomController.PointAction(pressed);
    }

    public struct ControllerValues
    {
        public bool TriggerPressed { get; internal set; }

        public bool GripPressed { get; internal set; }

        public Vector2 JoystickAxis { get; internal set; }

        public bool JoystickPressed { get; internal set; }

        public bool PrimaryButtonPressed { get; internal set; }

        public Quaternion Rotation { get; internal set; }

        public ControllerValues(bool triggerPressed, bool gripPressed, Vector2 joystick, bool joystickPressed, bool primaryButton, Quaternion rotation)
        {
            TriggerPressed = triggerPressed;
            GripPressed = gripPressed;
            JoystickPressed = joystickPressed;
            JoystickAxis = joystick;
            PrimaryButtonPressed = primaryButton;
            Rotation = rotation;
        }
    }
}
