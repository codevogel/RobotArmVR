using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material materialBloom;
    [SerializeField] private Material materialOG;

    public void HighlightObject()
    {
        transform.GetComponent<MeshRenderer>().material = materialBloom;
    }

    public void DeHighlightObject()
    {
        transform.GetComponent<MeshRenderer>().material = materialOG;
    }
}
