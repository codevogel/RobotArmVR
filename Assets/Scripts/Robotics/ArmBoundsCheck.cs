using IKManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBoundsCheck : MonoBehaviour
{
    [SerializeField]
    private CustomIKManager customIKManager;

    public bool FreeMovement { get; private set; }

    public bool debug;

    private void Start()
    {
        FreeMovement = true;
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
            customIKManager.inBounds = FreeMovement;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArmBounds"))
        {
            FreeMovement = true;
            customIKManager.inBounds = FreeMovement;
        }
    }
}
