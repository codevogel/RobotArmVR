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

    [field: SerializeField]
    private Transform canvas;

    private bool emergencyState;

    private void Start()
    {
        light = transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate(bool on)
    {
        emergencyState = RobotArmController.emergencyStop;
        if (!emergencyState)
        {
            light.gameObject.SetActive(on);
            audioSource.PlayOneShot(clip);
            return;
        }
        light.gameObject.SetActive(true);
    }
    
    public void ActivateCanvas(bool on)
    {
        canvas.gameObject.SetActive(on);
    }
}
