using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ControlDirectorTime))]
public class ControlDirectorTimeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var sdt = this.target as ControlDirectorTime;
        if (GUILayout.Button("Set time"))
        {
            sdt.SetTime();
        }
        if (GUILayout.Button("Pause"))
        {
            sdt.Pause();
        }
        if (GUILayout.Button("Resume"))
        {
            sdt.Resume();
        }
    }
}
