using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material materialBloom;
    private List<Transform> children = new List<Transform>();

    private BoxCollider boxCollider;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            children.Add(child.GetChild(3));
        }


    }

    public void HighlightObject(int trigger)
    {
        children[trigger].GetComponent<MeshRenderer>().material = materialBloom;
    }

    public void ActivateDoor(int trigger)
    {
        Debug.Log("In");
        boxCollider = transform.GetChild(trigger).GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        Debug.Log("Out");
    }
}
