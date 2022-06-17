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

    /// <summary>
    /// Activates IRC5
    /// </summary>
    /// <param name="on"></param>
    public void Activate(bool on)
    {
        // Is activated because of emergency?
        emergencyState = RobotArmController.emergencyStop;

        if (!emergencyState)
        {
            // Turn on light based on bool and play sound
            light.gameObject.SetActive(on);
            audioSource.PlayOneShot(clip);
            return;
        }
        // Turn on light
        light.gameObject.SetActive(true);
    }
    
    public void ActivateCanvas(bool on)
    {
        canvas.gameObject.SetActive(on);
    }
}
