using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRobotRotations : MonoBehaviour
{
    [SerializeField] private LinearMovement linearMovement;
    [SerializeField] private ArticulationBody[] articulationBodies;
    private ArticulationDrive[] articulationDrives;

    // Start is called before the first frame update
    void Start()
    {
        articulationDrives = new ArticulationDrive[articulationBodies.Length];
        for (int x=0; x<articulationBodies.Length;x++)
        {
            articulationDrives[x] = articulationBodies[x].xDrive;
        }
    }

    public void ResetRotations()
    {
        for (int x = 0; x < articulationBodies.Length; x++)
        {
            articulationBodies[x].xDrive = articulationDrives[x];
            StartCoroutine(WaitForDrive());
        }
    }

    private IEnumerator WaitForDrive()
    {
        yield return new WaitForSeconds(0.2f);
        linearMovement.followTarget[linearMovement.currentRobot].position = articulationBodies[5].transform.position;
    }
}
