using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PhaseITrigger : MonoBehaviour
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
        if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
        {
            Timeline.Pause();
            Timeline.time = 4395 / 60f;
            Timeline.Resume();
        }
    }
}
