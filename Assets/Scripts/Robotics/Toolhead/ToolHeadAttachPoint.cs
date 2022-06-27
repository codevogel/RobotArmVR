using UnityEngine;

/// <summary>
/// A component that relays trigger enter events to the toolhead controller.
/// </summary>
public class ToolHeadAttachPoint : MonoBehaviour
{
    /// <summary>
    /// The toolhead controller to relay messages to.
    /// </summary>
    [SerializeField, Tooltip("The toolhead controller to relay messages to.")] 
    private ToolHeadController _controller;

    /// <summary>
    /// Handles trigger messages and determine if they have to be relayed.
    /// </summary>
    /// <param name="other"> The object that has entered the trigger area.</param>
    private void OnTriggerEnter(Collider other)
    {
        // filter out for tool heads
        if (other.gameObject.TryGetComponent<ToolHeadBase>(out var tool))
        {
            _controller.HandleToolEnteredAttachArea(tool);
        }
    }
}
