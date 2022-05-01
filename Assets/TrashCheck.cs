using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Can"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ControllerLeft"))
        {
            gameObject.SetActive(false);
        }
    }
}
