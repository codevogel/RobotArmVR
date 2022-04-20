using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoopingSoundPlayer))]
public class LoopingSoundPlayerEditor : Editor
{
    AudioSource _audioSource;

    private void OnEnable()
    {
        _audioSource = ((LoopingSoundPlayer)target).GetComponent<AudioSource>();
    }

    public override void OnInspectorGUI()
    {
        var targetLSP = (LoopingSoundPlayer)target;
        int clipSamples = _audioSource.clip?.samples ?? default;

        // use float instead of int, because MinMaxSlider only accepts floats
        float minVal = targetLSP.IntroEndTime;
        float maxVal = targetLSP.OutroStartTime;

        EditorGUILayout.PrefixLabel("Set ");
        EditorGUILayout.BeginHorizontal();
        minVal = EditorGUILayout.FloatField(minVal);
        EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, 0, clipSamples);
        maxVal = EditorGUILayout.FloatField(maxVal);
        EditorGUILayout.EndHorizontal();

        if (minVal < 0)
            minVal = 0;

        if (maxVal > clipSamples)
            maxVal = clipSamples;

        targetLSP.IntroEndTime = Mathf.FloorToInt(minVal);
        targetLSP.OutroStartTime = Mathf.FloorToInt(maxVal);
    }
}
