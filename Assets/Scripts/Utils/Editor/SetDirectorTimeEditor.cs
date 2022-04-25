using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetDirectorTime))]
public class SetDirectorTimeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set time"))
        {
            var sdt = this.target as SetDirectorTime;
            sdt.SetTime();
        }
        base.OnInspectorGUI();
    }
}
