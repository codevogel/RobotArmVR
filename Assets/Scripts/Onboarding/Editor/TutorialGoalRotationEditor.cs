using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialGoalRotation))]
public class TutorialGoalRotationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tgp = base.target as TutorialGoalRotation;

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

        if (GUILayout.Button("Begin"))
        {
            tgp.Begin();
        }

        if (GUILayout.Button("Advance"))
        {
            tgp.AdvanceImmediately();
        }

        EditorGUI.EndDisabledGroup();

        base.OnInspectorGUI();
    }
}
