using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resets the robot to a neutral beginning position
/// </summary>
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

    /// <summary>
    /// Resets the articulationbodies drive of the robot
    /// </summary>
    public void ResetRotations()
    {
        for (int x = 0; x < articulationBodies.Length; x++)
        {
            articulationBodies[x].xDrive = articulationDrives[x];
            StartCoroutine(WaitForDrive());
        }
    }

    /// <summary>
    /// Small delay between the articulationdrive change to avoid overriding by the articulationbody
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForDrive()
    {
        yield return new WaitForSeconds(0.2f);
        linearMovement.followTarget[linearMovement.currentRobot].position = articulationBodies[5].transform.position;
    }
}
