using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{

    public Transform[] bones = new Transform[8];

    public int selectedBone = 0;

    public float moveSpeed;
    public float rotateSpeed;

    public float modifier;

    private Vector3 axis;

    private void FixedUpdate()
    {
        if (modifier != 0)
        {
            bones[selectedBone - 1].Rotate(axis, rotateSpeed * modifier * Time.deltaTime);
        }
    }

    public void SwitchAxis(InputAction.CallbackContext context)
    {
        // Get ascii character from control name and decrement by ascii value of 0
        selectedBone = context.control.name[0] - 48;
    }

    public void RotateAlong(InputAction.CallbackContext context)
    {
        if (selectedBone == 1 || selectedBone == 2)
        {
            axis = Vector3.forward;
        }
        else if (selectedBone == 4 || selectedBone == 6)
        {
            axis = Vector3.up;
        }
        else if (selectedBone == 3 || selectedBone == 5)
        {
            axis = Vector3.right;
        }
        else
        {
            throw new ArgumentException("Selected bone case unsupported.");
        }
        modifier = context.ReadValue<float>();
    }
}
