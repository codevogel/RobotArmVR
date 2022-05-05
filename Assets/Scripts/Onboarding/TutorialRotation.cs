using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TutorialRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateX(float xRotation)
    {
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.y, transform.rotation.z);
    }

    public void RotateY(float yRotation)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void RotateZ(float zRotation)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRotation);
    }
}
