using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material materialBloom;
    [SerializeField] private Material materialOG;

    public void HighlightObject(int trigger)
    {
        switch (trigger)
        {
            case 0:
                if (CompareTag("Hue1"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialBloom;
                }
                break;

            case 1:
                if (CompareTag("Hue2"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialBloom;
                }
                break;

            case 2:
                if (CompareTag("Hue3"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialBloom;
                }
                break;
        }
    }

    public void DeHighlightObject(int trigger)
    {
        switch (trigger)
        {
            case 0:
                if (CompareTag("Hue1"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialOG;
                }
                break;

            case 1:
                if (CompareTag("Hue2"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialOG;
                }
                break;

            case 2:
                if (CompareTag("Hue3"))
                {
                    transform.GetComponent<MeshRenderer>().material = materialOG;
                }
                break;
        }
    }
}
