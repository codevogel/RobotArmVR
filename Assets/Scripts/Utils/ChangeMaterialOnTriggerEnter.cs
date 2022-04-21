using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material material;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MeshRenderer>()!=null)
        {
            other.transform.GetComponent<MeshRenderer>().material = material;
        }
    }
}
