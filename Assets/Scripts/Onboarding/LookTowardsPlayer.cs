using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    private int offset = 90;

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion rotation=Quaternion.LookRotation(transform.position-Camera.main.transform.position, transform.up);
        rotation=Quaternion.Euler(0, rotation.eulerAngles.y- offset, 0);
        transform.rotation = rotation;
    }
}
