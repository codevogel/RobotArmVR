using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioSource : MonoBehaviour
{
    public AudioClip buttonPress, buttonRelease;

    [SerializeField]
    private AudioSource buttonAudioSource;

    private void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonPress()
    {
        buttonAudioSource.PlayOneShot(buttonPress);
    }

    public void PlayButtonRelease()
    {
        buttonAudioSource.PlayOneShot(buttonRelease);
    }
}
