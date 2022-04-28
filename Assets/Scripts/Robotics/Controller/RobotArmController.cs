using IKManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static HandManager;

public class RobotArmController : MonoBehaviour
{

    #region axis selection
    public Transform[] bones = new Transform[6];
    private Vector3 axis;
    public int selectedBone = 0;
    private bool axisSetOne = true;
    #endregion

    #region Movement modifiers
    public float moveSpeed;
    public float rotateSpeed;

    private float directionModifier;
    #endregion

    private bool movementOnLinear = true;

    private bool pressureButtonHeld = false;

    private JoystickInteractor joystickInteractor;

    [SerializeField]
    private CustomIKManager IKManager;

    [HideInInspector]
    public CustomInteractor Interactor;

    private LinearMovement linearMovement;

    private void Start()
    {
        joystickInteractor = HandManager.Instance.RightController.GetComponent<JoystickInteractor>();
        Interactor = GetComponent<CustomInteractor>();
        linearMovement = GetComponent<LinearMovement>();
        FlexpendantUIManager.Instance.SetAxis(bones);
        FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);
        IKManager.movementEnabled = movementOnLinear;
    }

    private void FixedUpdate()
    {
        if (pressureButtonHeld)
        {
            MoveArm();
            FlexpendantUIManager.Instance.UpdateAxis(selectedBone, bones[selectedBone]);
        }
    }

    /// <summary>
    /// Update axes
    /// </summary>
    public void ChangeAxisAction(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            return;
        }
        if (input.Equals(true))
        {
            axisSetOne = !axisSetOne;
            FlexpendantUIManager.Instance.ChangeAxisSet(axisSetOne);
        }
    }

    public void ChangeMovementMode(bool input, HandType leftRight)
    {
        if (leftRight.Equals(HandType.LEFT))
        {
            return;
        }
        if (input.Equals(true))
        {
            movementOnLinear = !movementOnLinear;
            FlexpendantUIManager.Instance.ChangeDirectionDisplay(movementOnLinear);
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
            pressureButtonHeld = input;
        }
    }

    /// <summary>
    /// Alters the selection and directionModifier
    /// </summary>
    private void MoveArm()
    {
        if (movementOnLinear)
        {
            LinearMovement();
            return;
        }
        ManualMovement();
    }

    private void LinearMovement()
    {
        if (IKManager.enabled==false)
        {
            IKManager.movementEnabled = true;
            linearMovement.enabled = true;

            foreach (Transform axis in bones)
            {
                axis.GetComponent<ArticulationBody>().enabled = true;
            }
        }
        
        Vector3 direction = new Vector3();

        if (joystickInteractor.joystickPressed)
        {
            direction.z = joystickInteractor.TiltAngle > 0 ? 1f : -1f;
        }
        else
        {
            direction.x = PlayerController.Right.JoystickAxis.x;
            direction.y = PlayerController.Right.JoystickAxis.y;
        }
        linearMovement.MoveTowards(direction);
    }

    private void ManualMovement()
    {
        if (IKManager.enabled == true)
        {
            IKManager.movementEnabled = true;
            linearMovement.enabled = false;

            foreach (Transform axis in bones)
            {
                axis.GetComponent<ArticulationBody>().enabled = false;
            }
        }

        bool move = false;
        Vector2 joystickInput = PlayerController.Right.JoystickAxis;

        if (joystickInteractor.joystickPressed)
        {
            if (Math.Abs(joystickInteractor.TiltAngle) > joystickInteractor.TiltAllowance)
            {
                move = true;
                axis = axisSetOne ? -Vector3.right : -Vector3.up;
                selectedBone = axisSetOne ? 2 : 5;
                directionModifier = joystickInteractor.TiltAngle > 0 ? 1f : -1f;
            }
        }
        else if (Math.Abs(joystickInput.x) > 0.01f || Math.Abs(joystickInput.y) > 0.01f)
        {
            bool modifyingX = Math.Abs(joystickInput.x) > Math.Abs(joystickInput.y) ? true : false;

            if (modifyingX)
            {
                move = true;
                axis = axisSetOne ? -Vector3.up : Vector3.up;
                selectedBone = axisSetOne ? 0 : 3;
                directionModifier = joystickInput.x > 0 ? 1f : -1f;
            }
            else
            {
                move = true;
                axis = axisSetOne ? -Vector3.forward : Vector3.right;
                selectedBone = axisSetOne ? 1 : 4;
                directionModifier = joystickInput.y > 0 ? 1f : -1f;
            }
        }
        if (move)
        {
            bones[selectedBone].Rotate(axis, rotateSpeed * directionModifier * Time.deltaTime);
        }
    }
}
