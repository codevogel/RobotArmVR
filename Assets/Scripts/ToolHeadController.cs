using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHeadController : MonoBehaviour
{
    [field: SerializeField] public Collider ToolHeadAttachPoint { get; set; }
    [field: SerializeField] public ToolHeadBase CurrentTool { get; set; }
}
