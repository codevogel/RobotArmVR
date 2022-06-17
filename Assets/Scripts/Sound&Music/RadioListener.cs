using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioListener : MonoBehaviour
{

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal void StopPlaying()
    {
        audioSource.Pause();
    }

    internal void StartPlaying(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Get sample time
    /// </summary>
    /// <returns>The current sample time</returns>
    internal int GetTime()
    {
        return audioSource.timeSamples;
    }

    /// <summary>
    /// Set current sample time
    /// </summary>
    internal void SetTime(int time)
    {
        audioSource.timeSamples = time;
    }
}
