using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays sound effects when the robot is moving
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RobotAudio : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.loop = true;
    }

    /// <summary>
    /// Start the robot sound effects
    /// </summary>
    public void StartLoop()
    {
        audioSource.mute = false;
        audioSource.Play();
    }
    
    /// <summary>
    /// Stop the robot sound effects
    /// </summary>
    public void Stop()
    {
        audioSource.Pause();
        audioSource.mute = true;
        audioSource.clip = clips[Random.Range(0, clips.Length)];
    }
}
