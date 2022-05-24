using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Triggers : MonoBehaviour
{
    [SerializeField] PlayableDirector Timeline;

    // Start is called before the first frame update
    void Awake()
    {
        Timeline = Timeline.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Teleport"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 2563 / 60f;
                Timeline.Resume();
            }
        }

        if (CompareTag("P1"))
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                Timeline.Pause();
                Timeline.time = 4395 / 60f;
                Timeline.Resume();
            }
        }
    }
}
