using UnityEngine;

/// <summary>
/// Handles when a tool should be marked as attachable again.
/// </summary>
public class ToolHeadAttachResetArea : MonoBehaviour
{
    /// <summary>
    /// The object the toolheads are attached to on the robot arm.
    /// </summary>
    private ToolHeadAttachPoint toolHeadAttach;

    /// <summary>
    /// Register the tool head attach point.
    /// </summary>
    private void Awake()
    {
        toolHeadAttach = transform.parent.GetComponentInChildren<ToolHeadAttachPoint>();
    }

    /// <summary>
    /// Handles when an object leaves the reset area.
    /// </summary>
    /// <param name="other"> The object that leaves the reset area.</param>
    private void OnTriggerExit(Collider other)
    {
        // make sure the object is a tool head and was attached to the attach point
        if (toolHeadAttach.transform != other.transform.parent && other.gameObject.TryGetComponent<ToolHeadBase>(out var tool))
        {
            tool.Attachable = true;
        }
    }
}
