using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerColliderFilter : MonoBehaviour
{
    [field: SerializeField]
    public Collider Filter { get; set; }
    [field:SerializeField] public UnityEvent OnTriggerEnterRelay { get; private set; }
    [field:SerializeField] public UnityEvent OnTriggerExitRelay { get; private set; }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other == Filter)
            OnTriggerEnterRelay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == Filter)
            OnTriggerExitRelay.Invoke();
    }
}
