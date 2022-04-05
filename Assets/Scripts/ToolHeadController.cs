using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadController : MonoBehaviour
{
    [field: SerializeField] public Collider ToolHeadAttachPoint { get; set; }
    [field: SerializeField] public ToolHeadBase CurrentTool { get; set; }

    public void HandleToolEnteredAttachArea(ToolHeadBase tool)
    {
        if(CurrentTool == null)
        {
            CurrentTool = tool;
            tool.AttachTool(this);
            tool.Rigidbody.isKinematic = true;
            tool.Rigidbody.useGravity = false;
        }
    }
}
