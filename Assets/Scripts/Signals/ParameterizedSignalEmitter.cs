using UnityEngine;
using UnityEngine.Timeline;

public abstract class ParameterizedSignalEmitter<T> : SignalEmitter
{
    [field: SerializeField] public T Parameter { get; set; }
}

