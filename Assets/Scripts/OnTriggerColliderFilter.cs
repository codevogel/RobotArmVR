using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerColliderFilter : MonoBehaviour
{
    [field: SerializeField]
    public Collider Filter { get; set; }
    [SerializeField] private UnityEvent _onTriggerEnter;
    [SerializeField] private UnityEvent _onTriggerExit;

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
            _onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == Filter)
            _onTriggerExit.Invoke();
    }
}
