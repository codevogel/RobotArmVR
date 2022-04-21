using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RobotFollowTarget : MonoBehaviour
{
    /// <summary>
    /// Radius in which the robot has to be to have reached it's target destination.
    /// </summary>
    [field: SerializeField]
    public float Radius { get; set; }

}
