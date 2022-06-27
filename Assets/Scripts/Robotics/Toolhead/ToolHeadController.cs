using UnityEngine;

/// <summary>
/// The authoring component for tool heads on a robot arm.
/// </summary>
public class ToolHeadController : MonoBehaviour
{
    /// <summary>
    /// The object on which to attach tool heads on.
    /// </summary>
    [field: SerializeField, Tooltip("The object on which to attach tool heads on.")] 
    public Collider ToolHeadAttachPoint { get; set; }

    /// <summary>
    /// The current tool head attached to the end effector.
    /// </summary>
    public ToolHeadBase CurrentTool { get; set; }

    /// <summary>
    /// Handles when a tool head enters the attach area.
    /// </summary>
    /// <param name="tool"> The tool that entered the attach area.</param>
    public void HandleToolEnteredAttachArea(ToolHeadBase tool)
    {
        // attach the tool to the end effector if there is no tool head attached at the moment.
        if(CurrentTool == null)
        {
            tool.AttachTool(this);
        }
    }
}
