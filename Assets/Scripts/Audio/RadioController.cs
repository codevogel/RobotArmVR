using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    AudioSource _audioSource;
    bool _isPlaying = true;

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
            // toggle radio aan/uit

            if (_isPlaying)
            {
                _audioSource.Stop();
            }
            else
            {
                _audioSource.UnPause();
            }

            // ! maakt van true false en false true
            _isPlaying = !_isPlaying;
        }
    }
}