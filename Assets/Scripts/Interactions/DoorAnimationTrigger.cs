using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens and closes the doors in the scene 
/// </summary>
public class DoorAnimationTrigger : MonoBehaviour
{
    Animator _doorAnim;
    private List<Collider> collided =  new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        _doorAnim = this.transform.GetComponentInParent<Animator>();
    }

    /// <summary>
    /// Opens the door when an object enters its range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        collided.Add(other);
        _doorAnim.SetBool("isOpen", true);
    }

    /// <summary>
    /// Closes the door when no object is in its range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        collided.Remove(other);
        if (collided.Count==0)
        {
            _doorAnim.SetBool("isOpen", false);
        }
    }
}
