using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    [SerializeField] private GameObject audioSourceR1;
    [SerializeField] private GameObject audioSourceR2;
    [SerializeField] private GameObject audioSourceR3;
    bool soundToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        soundToggle = !soundToggle;
        if (soundToggle)
        {
            audioSourceR1.SetActive(true);
            audioSourceR2.SetActive(true);
            audioSourceR3.SetActive(true);
        }
        else
        {
            audioSourceR1.SetActive(false);
            audioSourceR2.SetActive(false);
            audioSourceR3.SetActive(false);
        }
    }
}
