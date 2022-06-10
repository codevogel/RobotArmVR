using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens the display case
/// </summary>
[RequireComponent(typeof(DisplayCaseOpen))]
public class DisplayCaseOpener : MonoBehaviour
{

    private Transform glassTop;
    public bool open;

    private void Start()
    {
        glassTop = transform.Find("Glass Top");
    }

    /// <summary>
    /// Sets flag to open display case
    /// </summary>
    public void Open()
    {
        open = true;
    }

    private void Update()
    {
        if (open)
        {
            glassTop.Rotate(Vector3.down, 1f);
            // If glasstop is open
            if (glassTop.localRotation.eulerAngles.x >= 350)
            {
                // Destroy script to prevent further polling
                Destroy(this);
            }
        }
    }
}
