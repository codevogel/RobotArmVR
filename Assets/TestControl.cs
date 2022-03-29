using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TestControl : MonoBehaviour
{


    private XRCustomController xrCustomController;

    private void Start()
    {
        xrCustomController = GetComponent<XRCustomController>();
        xrCustomController.thumbstickValueAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("anuspiemel");
        Debug.Log(obj.ReadValue<Vector2>());
    }
}
