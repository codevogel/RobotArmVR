using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Material materialBloom;
    private List<Transform> children = new List<Transform>();

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
}
