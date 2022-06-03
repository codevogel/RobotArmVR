using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    private int assistantOffset = 90;
    private bool active=true;

    // Update is called once per frame
    void LateUpdate()
    {
        if (active)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, transform.up);
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y - assistantOffset, rotation.eulerAngles.z);
            transform.rotation = rotation;
        }
    }

    public void Active(bool activate)
    {
        active = activate;
    }
}
