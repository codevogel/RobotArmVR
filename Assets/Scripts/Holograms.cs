using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holograms : MonoBehaviour
{
    [SerializeField] private Material hologramMaterial;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<MeshRenderer>().material =hologramMaterial;   
    }
}
