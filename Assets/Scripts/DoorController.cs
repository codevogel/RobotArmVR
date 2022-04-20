using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator _doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        _doorAnim = this.transform.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _doorAnim.SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        _doorAnim.SetBool("isOpen", false);
    }
}
