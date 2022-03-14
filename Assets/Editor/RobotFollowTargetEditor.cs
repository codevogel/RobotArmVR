using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RobotFollowTarget))]
public class RobotFollowTargetEditor : Editor
{

    private void OnSceneGUI()
    {
        RobotFollowTarget robotFollowTarget = (RobotFollowTarget) target;

        Handles.color = Color.green;
        Handles.DrawWireDisc(robotFollowTarget.transform.position, Vector3.up, robotFollowTarget.Radius);

    }
}
