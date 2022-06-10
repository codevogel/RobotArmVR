using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

        if (CompareTag("P3"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 13440 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("Poster"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 13810 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("Cage"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 16500 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("IRC5"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 17679 / 60f;
                Timeline.Resume();
            }
        }
    }
}
