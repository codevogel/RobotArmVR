using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportControls : MonoBehaviour
{
    private bool teleportActive;
    private XRCustomController controller;

    [SerializeField]
    private Material lineMaterial;

    private void Start()
    {
        controller = GetComponent<XRCustomController>();
        controller.teleportAction.action.performed+=OnTeleportButton;
        controller.teleportAction.action.canceled += OnTeleportButton;
    }

    public void OnTeleportButton(InputAction.CallbackContext obj)
    {
        if (!teleportActive)
        {
            RemoveComponents(transform.gameObject);
        }
        else
        {
            AddComponents(transform.gameObject);
        }
        teleportActive = !teleportActive;
    }

    private void RemoveComponents(GameObject hand)
    {
        Destroy(hand.GetComponent<CustomInteractor>());
        StartCoroutine("WaitForDestroy");
    }

    private IEnumerator WaitForDestroy()
    {
        yield return null;
        transform.gameObject.AddComponent<XRRayInteractor>();
        transform.gameObject.AddComponent<XRInteractorLineVisual>();
        transform.gameObject.GetComponent<LineRenderer>().material = lineMaterial;
    }

    private void AddComponents(GameObject hand)
    {
        Destroy(hand.GetComponent<XRRayInteractor>());
        Destroy(hand.GetComponent<XRInteractorLineVisual>());
        Destroy(hand.GetComponent<LineRenderer>());
        StartCoroutine("WaitForRestore");
    }

    private IEnumerator WaitForRestore()
    {
        yield return null;
        transform.gameObject.AddComponent<CustomInteractor>();
        CustomInteractor interactor = transform.GetComponent<CustomInteractor>();
        if (transform.name.Equals("RightHand Controller"))
        {
            transform.GetComponent<RobotController>().Interactor = interactor; 
        }
    }
}
