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
    public GameObject[] robotArms;
    private ArticulationBody[] articulationBodies = new ArticulationBody[6];
    private ArticulationJointController[] articulationJointControllers = new ArticulationJointController[6];
    public int selectedArticulator = 0;
    private bool axisSetOne = true;
    #endregion

    #region Movement modifiers
    public float rotateSpeed;

    private RotationDirection rotationDirection;

    public static bool emergencyStop;
    #endregion

    private bool movementOnLinear = true;

    private bool pressureButtonHeld = false;

    private JoystickInteractor joystickInteractor;

    [SerializeField]
    private CustomIKManager[] IKManagers;

    [HideInInspector]
    public CustomInteractor Interactor;

    private LinearMovement linearMovement;

    [SerializeField]
    private float joystickThreshold;

    [field: SerializeField]
    private RobotAudio[] robotAudio;

    [SerializeField]
    private List<PushButton> buttons;

    public UnityEvent OnPressureButtonDown;
    public UnityEvent OnPressureButtonUp;

    [SerializeField]
    private IRC5Controller irc5controller;

    private bool emergencyStopEnabled;
    private bool axisButtonEnabled;
    private bool incrementEnabled;
    private bool movementButtonEnabled=true;
    private bool firstStart=true;

    private bool armMoving;

    public int currentRobot;

    private void Awake()
    {
        RobotArmSwitch(robotArms[0]);
    }

    private void Start()
    {
        FlexpendantUIManager.Instance.SetAxis(articulationBodies);
        FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);

        joystickInteractor = HandManager.Instance.RightController.GetComponent<JoystickInteractor>();
        Interactor = GetComponent<CustomInteractor>();
        linearMovement = GetComponent<LinearMovement>();

        foreach (CustomIKManager ikManager in IKManagers)
        {
            ikManager.movementEnabled = movementOnLinear;
        }

        for (int x=1; x<IKManagers.Length;x++)
        {
            IKManagers[x].enabled = false;
        }

        for (int i = 0; i < articulationBodies.Length; i++)
        {
            articulationJointControllers[i] = articulationBodies[i].GetComponent<ArticulationJointController>();
        }
        ChangeMovementMode();
    }

    private void FixedUpdate()
    {
        bool moved = false;
        if (pressureButtonHeld && !emergencyStop)
        {
            moved = MoveArm();
        }

        if (!armMoving && moved)
        {
            armMoving = true;
            
            robotAudio[currentRobot].StartLoop();
        }
        if (armMoving && !moved)
        {
            armMoving = false;
            robotAudio[currentRobot].Stop();

            foreach (ArticulationJointController art in articulationJointControllers)
            {
                art.rotationState= RotationDirection.None;
            }
        }

        if (armMoving)
        {
            FlexpendantUIManager.Instance.UpdateAxis(selectedArticulator, articulationBodies[selectedArticulator].transform);
        }
    }

    private void RobotArmSwitch(GameObject robotarm)
    {
        for (int x = 0; x < articulationBodies.Length; x++)
        {
            if (x == 0)
            {
                articulationBodies[x] = robotarm.transform.GetChild(0).GetComponent<ArticulationBody>();
            }
            else
            {
                articulationBodies[x] = articulationBodies[x - 1].transform.GetChild(1).GetComponent<ArticulationBody>();
            }
        }
        for (int i = 0; i < articulationBodies.Length; i++)
        {
            articulationJointControllers[i] = articulationBodies[i].GetComponent<ArticulationJointController>();
        }
    }

    public void ChangeRobot(int robot)
    {
        currentRobot = robot;
        RobotArmSwitch(robotArms[robot]);
        linearMovement.ChangeRobot(robot);

        if (movementOnLinear)
        {
            if (robot == 0)
            {
                IKManagers[robot + 1].enabled = false;
            }
            else
            {
                IKManagers[robot - 1].enabled = false;
            }
            Debug.Log("reached");
            IKManagers[robot].enabled = true;
            linearMovement.followTarget[robot].position = articulationBodies[5].transform.position;
            return;
        }
        
    }

    public void SetEmergencyStop(bool stop)
    {
        if (emergencyStopEnabled)
        {
            emergencyStop = stop;
            irc5controller.Activate(stop);
            return;
        }
        //Unfreeze the emergency button when its not yet enabled
        buttons[4].FreezeButton(false);
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

    public void ResetButtons()
    {
        foreach (PushButton button in buttons)
        {
            button.Reset();
        }
    }

    /// <summary>
    /// Update axes
    /// </summary>
    public void ChangeAxisAction()
    {
        if (axisButtonEnabled)
        {
            axisSetOne = !axisSetOne;
            FlexpendantUIManager.Instance.ChangeAxisSet(axisSetOne);
        }
    }

    public void ChangeMovementMode()
    {
        if (movementButtonEnabled)
        {
            //If the function is called during the start
            if (firstStart)
            {
                movementButtonEnabled = false;
                firstStart = false;
            }

            movementOnLinear = !movementOnLinear;
            FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);

            if (movementOnLinear)
            {
                StopArticulation();
                IKManagers[currentRobot].enabled = true;
                linearMovement.enabled = true;
                linearMovement.followTarget[currentRobot].position = articulationBodies[5].transform.position;

                linearMovement.RecalculatePreviousAngle();
            }
            else
            {
                StopArticulation();
                IKManagers[currentRobot].enabled = false;
                linearMovement.enabled = false;
            }
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
    /// Enables a buttons functionality depending on the number given
    /// </summary>
    /// <param name="buttonNumber">The button that needs to be enabled</param>
    public void EnableButtonFunction(int buttonNumber)
    {
        switch (buttonNumber)
        {
            //Enable the switch axis button
            case 0:
                axisButtonEnabled = true;
                break;
            //Enable the switch movement mode button
            case 1:
                movementButtonEnabled = true;
                break;
            //Enable the emergency stop button
            case 2:
                emergencyStopEnabled = true;
                break;
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
