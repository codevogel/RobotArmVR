using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <summary>
/// A component to filter trigger events to be handled with a specific colider.
/// </summary>
[RequireComponent(typeof(Collider))]
public class OnTriggerColliderFilter : MonoBehaviour
{
    /// <summary>
    /// The collider the respond to.
    /// </summary>
    [field: SerializeField, Tooltip("The collider the respond to.")]
    public Collider Filter { get; set; }

    /// <summary>
    /// The actions to execute when the collider has entered the trigger area.
    /// </summary>
    [field: SerializeField, Tooltip("The actions to execute when the collider has entered the trigger area.")]
    public UnityEvent OnTriggerEnterRelay { get; private set; }

    /// <summary>
    /// The actions to execute when the collider has left the trigger area.
    /// </summary>
    [field: SerializeField, Tooltip("The actions to execute when the collider has left the trigger area.")]
    public UnityEvent OnTriggerExitRelay { get; private set; }

#if UNITY_EDITOR
    private void Awake()
    {
        var colliders = GetComponents<Collider>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
                return;
        }

        Debug.LogWarning($"\"{this.gameObject.name}\" does not have a trigger collider and will never fire \"OnTriggerEnter\".");
    }
#endif

    /// <summary>
    /// Handles when a collider entered the trigger area.
    /// </summary>
    /// <param name="other"> The collider that has entered the area.</param>
    private void OnTriggerEnter(Collider other)
    {
        // make sure the collider is the right collider
        if (other == Filter)
            OnTriggerEnterRelay.Invoke();
    }

    /// <summary>
    /// Handles when a collider left the trigger area.
    /// </summary>
    /// <param name="other"> The collider that has left the area.</param>
    private void OnTriggerExit(Collider other)
    {
        // make sure the collider is the right collider
        if (other == Filter)
            OnTriggerExitRelay.Invoke();
    }
}
