using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Swaps the active robot which is being controlled
/// </summary>
[RequireComponent(typeof(Swapper))]
public class RobotSwapping : MonoBehaviour
{
    [SerializeField] private RobotArmController robotArmController;

    private Room currentRoom;
    private Swapper swapper;

    private void Start()
    {
        swapper = GetComponent<Swapper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrainingRoom") && currentRoom != Room.TRAINING)
        {
            currentRoom = Room.TRAINING;
            swapper.Swap();
            robotArmController.ChangeRobot(0);
        }
        if (other.CompareTag("ExpertRoom") && currentRoom != Room.EXPERT)
        {
            currentRoom = Room.EXPERT;
            swapper.Swap();
            robotArmController.ChangeRobot(1);
        }
    }

    private enum Room
    {
        TRAINING,
        EXPERT
    }
}
