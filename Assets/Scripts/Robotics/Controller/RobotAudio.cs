using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartLoop()
    {
        audioSource.mute = false;
        audioSource.Play();
    }
    
    public void Stop()
    {
        audioSource.Pause();
        audioSource.mute = true;
        audioSource.clip = clips[Random.Range(0, clips.Length)];
    }
}
