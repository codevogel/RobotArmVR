using IKManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static HandManager;

public class RobotArmController : MonoBehaviour
{

    #region axis selection
    public ArticulationBody[] articulationBodies = new ArticulationBody[6];
    private ArticulationJointController[] articulationJointControllers = new ArticulationJointController[6];
    private Vector3 axis;
    public int selectedArticulator = 0;
    private bool axisSetOne = true;
    #endregion

    #region Movement modifiers
    public float moveSpeed;
    public float rotateSpeed;

    private RotationDirection rotationDirection;

    public static bool emergencyStop;
    #endregion

    private bool movementOnLinear = true;

    private bool pressureButtonHeld = false;

    private JoystickInteractor joystickInteractor;

    [SerializeField]
    private CustomIKManager IKManager;

    [HideInInspector]
    public CustomInteractor Interactor;

    private LinearMovement linearMovement;

    [SerializeField]
    private float joystickThreshold;

    [SerializeField]
    private List<PushButton> buttons;


    public UnityEvent OnPressureButtonDown;
    public UnityEvent OnPressureButtonUp;

    private bool armMoving;
    [field: SerializeField]
    private RobotAudio robotAudio;

    private void Start()
    {
        joystickInteractor = HandManager.Instance.RightController.GetComponent<JoystickInteractor>();
        Interactor = GetComponent<CustomInteractor>();
        linearMovement = GetComponent<LinearMovement>();

        FlexpendantUIManager.Instance.SetAxis(articulationBodies);
        FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);
        IKManager.movementEnabled = movementOnLinear;


        for (int i = 0; i < articulationBodies.Length; i++)
        {
            articulationJointControllers[i] = articulationBodies[i].GetComponent<ArticulationJointController>();
        }
    }

    private void FixedUpdate()
    {
        bool moved = false;
        if (pressureButtonHeld && !emergencyStop)
        {
            moved = MoveArm();
            FlexpendantUIManager.Instance.UpdateAxis(selectedArticulator, articulationBodies[selectedArticulator].transform);
        }

        if (!armMoving && moved)
        {
            armMoving = true;
            robotAudio.StartLoop();
        }
        if (armMoving && !moved)
        {
            armMoving = false;
            robotAudio.Stop();
        }
    }

    public void SetEmergencyStop(bool stop)
    {
        emergencyStop = stop;
    }

    
    public void EnableButtons(bool enabled)
    {
        foreach (PushButton button in buttons)
        {
            button.rb.isKinematic = !enabled;
            if (button.frozen)
            {
                button.rb.isKinematic = true;
            }
        }
    }

    /// <summary>
    /// Update axes
    /// </summary>
    public void ChangeAxisAction()
    {
        axisSetOne = !axisSetOne;
        FlexpendantUIManager.Instance.ChangeAxisSet(axisSetOne);
    }

    public void ChangeMovementMode()
    {
        movementOnLinear = !movementOnLinear;
        FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);

        if (movementOnLinear)
        {
            StopArticulation();
            IKManager.enabled = true;
            linearMovement.enabled = true;
            linearMovement.followTarget.position = articulationBodies[5].transform.position;
        }
        else
        {
            StopArticulation();
            IKManager.enabled = false;
            linearMovement.enabled = false;
        }
    }


    /// <summary>
    /// Used to control the pressure button.
    /// </summary>
    /// <param name="input">Whether the trigger button is held</param>
    /// <param name="leftRight">On which hand?</param>
    public void SetPressureButton(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.RIGHT))
        {
            return;
        }

        Transform heldDevice = HandManager.Instance.GetHeldObject(HandManager.HandType.LEFT);

        if (heldDevice == null)
            return;


        if (heldDevice.transform.name == "Flexpendant")
        {
            if (pressureButtonHeld != input)
            {
                if (input)
                {
                    OnPressureButtonDown.Invoke();
                }
                else
                {
                    OnPressureButtonUp.Invoke();
                }
            }
            pressureButtonHeld = input;
        }
    }


    /// <summary>
    /// Alters the selection and directionModifier
    /// </summary>
    private bool MoveArm()
    {
        bool moved;
        if (movementOnLinear)
        {
            LinearMovement(out moved);
        }
        else
        {
            ManualMovement(out moved);
        }

        return moved;
    }

    private void LinearMovement(out bool moved)
    {
        Vector3 direction = new Vector3();

        if (joystickInteractor.joystickPressed)
        {
            float modifier = joystickInteractor.TiltAngle > 0 ? 1f : -1f;
            direction.y = Mathf.Clamp(Math.Abs(joystickInteractor.TiltAngle / 180f), .5f, 1f) * modifier;
        }
        else
        {
            direction.x = PlayerController.Right.JoystickAxis.y;
            direction.z = -PlayerController.Right.JoystickAxis.x;
        }
        moved = direction != Vector3.zero;
        linearMovement.MoveTowards(direction);
    }

    private void ManualMovement(out bool moved)
    {
        Vector2 joystickInput = PlayerController.Right.JoystickAxis;

        HandleInput(out moved, joystickInput);

        if (moved)
        {
            articulationBodies[selectedArticulator].GetComponent<ArticulationJointController>().rotationState = rotationDirection;
        }
        else
        {
            StopArticulation();
        }
    }

    private void HandleInput(out bool move, Vector2 joystickInput)
    {
        if (joystickInteractor.joystickPressed)
        {
            // Check whether rotation is more than threshold
            if (Math.Abs(joystickInteractor.TiltAngle) > joystickInteractor.TiltThreshold)
            {
                move = true;
                selectedArticulator = axisSetOne ? 2 : 5;
                rotationDirection = joystickInteractor.TiltAngle > 0 ? RotationDirection.Positive : RotationDirection.Negative;
                return;
            }
        }
        else if (joystickInput.magnitude > joystickThreshold)
        {
            bool modifyingX = Math.Abs(joystickInput.x) > Math.Abs(joystickInput.y) ? true : false;

            if (modifyingX)
            {
                move = true;
                selectedArticulator = axisSetOne ? 0 : 3;
                rotationDirection = joystickInput.x > 0 ? RotationDirection.Negative : RotationDirection.Positive;
                return;
            }
            else
            {

                move = true;
                selectedArticulator = axisSetOne ? 1 : 4;
                if (selectedArticulator == 1)
                {
                    rotationDirection = joystickInput.y > 0 ? RotationDirection.Negative : RotationDirection.Positive;
                }
                else
                {
                    rotationDirection = joystickInput.y > 0 ? RotationDirection.Positive : RotationDirection.Negative;
                }
                return;
            }
        }
        move = false;
    }

    private void StopArticulation()
    {
        foreach (ArticulationJointController articulationController in articulationJointControllers)
        {
            articulationController.rotationState = RotationDirection.None;
        }
    }
}
