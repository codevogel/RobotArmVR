using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holograms : MonoBehaviour
{
    [SerializeField] private Material hologramMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Here");
        other.transform.GetComponent<MeshRenderer>().material =hologramMaterial;   
    }
}
