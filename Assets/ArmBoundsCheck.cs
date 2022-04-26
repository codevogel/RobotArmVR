using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBoundsCheck : MonoBehaviour
{

    public bool FreeMovement { get; private set; }

    private void Start()
    {
        FreeMovement = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ArmBounds"))
        {
            FreeMovement = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArmBounds"))
        {
            FreeMovement = true;
        }
    }
}
