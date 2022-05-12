using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseITrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
        {
            gameObject.SetActive(false);
        }
    }
}
