using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material materialBloom;
    [SerializeField] private Material materialOG;

    public void HighlightObject()
    {
        if (gameObject.CompareTag("Hue"))
        {
            transform.GetComponent<MeshRenderer>().material = materialBloom;
        }
    }

    public void DeHighlightObject()
    {
        if (gameObject.CompareTag("Hue"))
        {
            transform.GetComponent<MeshRenderer>().material = materialOG;
        }
    }
}
