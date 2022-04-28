using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBoundsCheck : MonoBehaviour
{
    private LinearMovement linearMovement;

    public bool FreeMovement { get; private set; }

    public bool debug;

    private void Start()
    {
        FreeMovement = true;
        linearMovement = HandManager.Instance.LeftController.GetComponent<LinearMovement>();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, new Vector3(.25f, .25f, .25f));
        }
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
