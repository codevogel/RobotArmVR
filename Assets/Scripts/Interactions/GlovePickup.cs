using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the color of the hands after picking up the gloves
/// </summary>
[RequireComponent(typeof(DisplayCaseOpener))]
public class GlovePickup : MonoBehaviour
{
    private DisplayCaseOpener displayCaseOpener;

    private void Start()
    {
        displayCaseOpener = GetComponent<DisplayCaseOpener>();
    }

    /// <summary>
    /// Changes the color of the hands after picking up the gloves
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (displayCaseOpener.open)
        {
            if (other.CompareTag("ControllerLeft") || other.CompareTag("ControllerRight"))
            {
                GameObject.FindGameObjectWithTag("LeftFinger").GetComponent<GloveColourChanger>().ChangeColour();
                GameObject.FindGameObjectWithTag("RightFinger").GetComponent<GloveColourChanger>().ChangeColour();
                transform.Find("LeftHand").gameObject.SetActive(false);
                transform.Find("RightHand").gameObject.SetActive(false);
                Destroy(this);
            }
        }
    }
}
