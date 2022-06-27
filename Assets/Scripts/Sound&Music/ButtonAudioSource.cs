using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the button audio when needed
/// </summary>
public class ButtonAudioSource : MonoBehaviour
{
    public AudioClip buttonPress, buttonRelease;

    private AudioSource buttonAudioSource;

    private void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play the button audio of the button pressing
    /// </summary>
    public void PlayButtonPress()
    {
        buttonAudioSource.PlayOneShot(buttonPress);
    }

    /// <summary>
    /// Play the button audio of the button releasing
    /// </summary>
    public void PlayButtonRelease()
    {
        buttonAudioSource.PlayOneShot(buttonRelease);
    }
}
