using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Activate a trigger for the onboarding director to change to a specific phase,
///  values of the time can be found in the JSON file
/// </summary>
public class TutorialTriggers : MonoBehaviour
{
    [SerializeField] PlayableDirector Timeline;

    // Start is called before the first frame update
    void Awake()
    {
        Timeline = Timeline.GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Teleport"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 2218 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("P1"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 4174 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("Axes"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 5931 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("Poster"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 14095 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("IRC5"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 16995 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("Cage"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 17815 / 60f;
                Timeline.Resume();
            }
        }
    }
}
