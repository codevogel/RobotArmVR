using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCheck : MonoBehaviour
{
    Rigidbody rb2;

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
            other.gameObject.SetActive(false);
            rb2 = other.gameObject.GetComponent<Rigidbody>();
            rb2.useGravity = false;
        }
        else if (other.CompareTag("ControllerLeft"))
        {
            gameObject.SetActive(false);
        }
    }
}
