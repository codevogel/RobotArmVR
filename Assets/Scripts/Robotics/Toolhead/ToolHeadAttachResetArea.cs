using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadAttachResetArea : MonoBehaviour
{
    private ToolHeadAttachPoint toolHeadAttach;

    private void Awake()
    {
        toolHeadAttach = transform.parent.GetComponentInChildren<ToolHeadAttachPoint>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (toolHeadAttach.transform != other.transform.parent && other.gameObject.TryGetComponent<ToolHeadBase>(out var tool))
        {
            tool.Attachable = true;
        }
    }
}
