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

    internal int GetTime()
    {
        return audioSource.timeSamples;
    }

    internal void SetTime(int time)
    {
        audioSource.timeSamples = time;
    }
}
