using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControl : MonoBehaviour
{
    AudioSource _audioSource;
    bool _isPlaying = true;

    // Bij awake, vraagt naar audio source om af te spelen
    private void Awake()
    {
        _audioSource = transform.parent.GetComponent<AudioSource>();
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
        }
    }
}
