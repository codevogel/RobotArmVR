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

    private XRCustomController xrCustomController;

    [field: SerializeField]
    private TextUpdater textUpdater;

    private void Start()
    {
        xrCustomController = GetComponent<XRCustomController>();
        xrCustomController.thumbstickValueAction.action.performed += ThumbstickAction;
        xrCustomController.axisUpAction.action.started += ChangeAxisAction;
    }

    private void ChangeAxisAction(InputAction.CallbackContext obj)
    {
        selectedBone = (selectedBone + 1) % bones.Length;
        ChangeAxis();
        textUpdater.UpdateText((selectedBone + 1).ToString());
    }

    private void ThumbstickAction(InputAction.CallbackContext obj)
    {
        modifier = obj.ReadValue<Vector2>().x;
    }


    private void FixedUpdate()
    {
        if (Math.Abs(modifier) > 0.01)
        {
            Debug.Log(bones[selectedBone].rotation);
            bones[selectedBone].Rotate(axis, rotateSpeed * modifier * Time.deltaTime);
        }
    }

    public void SwitchAxis(InputAction.CallbackContext context)
    {
        // Get ascii character from control name and decrement by ascii value of 0
        selectedBone = context.control.name[0] - 48;
    }

    private void ChangeAxis()
    {
        if (selectedBone == 0 || selectedBone == 1)
        {
            axis = Vector3.forward;
        }
        else if (selectedBone == 3 || selectedBone == 5)
        {
            axis = Vector3.up;
        }
        else if (selectedBone == 2 || selectedBone == 4)
        {
            axis = Vector3.right;
        }
        else
        {
            throw new ArgumentException("Selected bone case unsupported.");
        }
    }
}
