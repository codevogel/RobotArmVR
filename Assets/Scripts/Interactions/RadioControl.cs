using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class RadioControl : MonoBehaviour
{
    AudioSource _audioSource;
    bool _isPlaying = true;

    [SerializeField] UnityEvent radioToggle;

    // Bij awake, vraagt naar audio source om af te spelen
    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Nu, wanneer collider met tag player collide, gaat de knop aan/uit
        if (other.CompareTag("ControllerRight") || other.CompareTag("ControllerLeft"))
        {
            if (_isPlaying)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.UnPause();
            }

            // ! maakt van true false en false true
            _isPlaying = !_isPlaying;
            radioToggle.Invoke();
        }
    }
}
