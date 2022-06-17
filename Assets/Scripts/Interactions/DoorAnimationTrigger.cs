using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationTrigger : MonoBehaviour
{
    Animator _doorAnim;
    private List<Collider> collided =  new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        _doorAnim = this.transform.GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        collided.Add(other);
        _doorAnim.SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        collided.Remove(other);
        if (collided.Count==0)
        {
            _doorAnim.SetBool("isOpen", false);
        }
    }
}
