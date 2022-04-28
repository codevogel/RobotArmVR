using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBoundsCheck : MonoBehaviour
{
    private LinearMovement linearMovement;

    public bool FreeMovement { get; private set; }

    private void Start()
    {
        FreeMovement = true;
        linearMovement = HandManager.Instance.LeftController.GetComponent<LinearMovement>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ArmBounds"))
        {
            FreeMovement = false;
            linearMovement.inBounds = FreeMovement;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArmBounds"))
        {
            FreeMovement = true;
            linearMovement.inBounds = FreeMovement;
        }
    }
}
