using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IRC5Controller : MonoBehaviour
{
    private Transform light;

    private AudioSource audioSource;

    [field: SerializeField]
    private AudioClip clip;

    private void Start()
    {
        light = transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate(bool on)
    {
        light.gameObject.SetActive(on);
        audioSource.PlayOneShot(clip);
    }

}
