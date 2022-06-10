using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCaseOpener : MonoBehaviour
{

    private Transform glassTop;
    public bool open;
    private float originalRotation;

    private void Start()
    {
        glassTop = transform.Find("Glass Top");

        originalRotation = glassTop.localRotation.x; 
    }

    public void Open()
    {
        open = true;
    }

    private void Update()
    {
        if (open)
        {
            glassTop.Rotate(Vector3.down, 1f);
            if (glassTop.localRotation.eulerAngles.x >= 350)
            {
                Destroy(this);
            }
        }
    }
}
