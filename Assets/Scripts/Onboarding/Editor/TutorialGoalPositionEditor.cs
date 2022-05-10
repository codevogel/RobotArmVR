using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialGoalPositions))]
public class TutorialGoalPositionEditor : Editor
{
    Collider _triggerFilter;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Editor only");
        var tgp = base.target as TutorialGoalPositions;

        EditorGUILayout.BeginHorizontal();

        _triggerFilter = EditorGUILayout.ObjectField("Filter", _triggerFilter, typeof(Collider), true) as Collider;

        EditorGUI.BeginDisabledGroup(_triggerFilter == null);

        if (GUILayout.Button("Set Filter"))
        {
            var prop = serializedObject.FindProperty("_colliders");

            int size = prop.arraySize;

            for (int i = 0; i < size; i++)
            {
                if (prop.GetArrayElementAtIndex(i).objectReferenceValue is OnTriggerColliderFilter triggerFilter)
                {
                    triggerFilter.Filter = _triggerFilter;
                }
            }
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

        if(GUILayout.Button("Begin"))
        {
            tgp.Begin();
        }

        if (GUILayout.Button("Advance"))
        {
            tgp.Advance();
        }

        EditorGUI.EndDisabledGroup();

        EditorHelper.DrawUILine(Color.gray);

        base.OnInspectorGUI();
    }
}