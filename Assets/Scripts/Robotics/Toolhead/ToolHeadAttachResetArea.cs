using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadAttachResetArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ToolHeadBase>(out var tool))
        {
            tool.Attachable = true;
        }
    }
}
