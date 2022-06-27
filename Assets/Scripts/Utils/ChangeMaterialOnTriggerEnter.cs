using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the huelights around the door entrance
/// </summary>
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

    /// <summary>
    /// Change the bloom of the correct door
    /// </summary>
    /// <param name="trigger">The correct door</param>
    public void HighlightObject(int trigger)
    {
        children[trigger].GetComponent<MeshRenderer>().material = materialBloom;
    }

    /// <summary>
    /// Allow the doors to be teleported through
    /// </summary>
    /// <param name="trigger">The correct door</param>
    public void ActivateDoor(int trigger)
    {
        boxCollider = transform.GetChild(trigger).GetComponent<BoxCollider>();
        boxCollider.enabled = true;
    }
}
