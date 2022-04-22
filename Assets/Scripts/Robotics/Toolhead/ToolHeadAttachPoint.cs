using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadAttachPoint : MonoBehaviour
{
    [SerializeField] private ToolHeadController _controller;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.TryGetComponent<ToolHeadBase>(out var tool))
        {
            _controller.HandleToolEnteredAttachArea(tool);
        }
    }
}
