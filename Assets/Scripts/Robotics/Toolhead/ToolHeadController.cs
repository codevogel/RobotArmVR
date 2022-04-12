using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadController : MonoBehaviour
{
    [field: SerializeField] public Collider ToolHeadAttachPoint { get; set; }
    public ToolHeadBase CurrentTool { get; set; }

    public void HandleToolEnteredAttachArea(ToolHeadBase tool)
    {
        if(CurrentTool == null)
        {
            tool.AttachTool(this);
        }
    }
}
