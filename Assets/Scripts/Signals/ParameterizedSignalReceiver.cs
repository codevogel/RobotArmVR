using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// A component to receive signals form the timeline with additional parameters.
/// </summary>
/// <typeparam name="T"> The parameter type attached to the signal to listen to.</typeparam>
[RequireComponent(typeof(SignalReceiver))]
public abstract class ParameterizedSignalReceiver<T> : MonoBehaviour, INotificationReceiver
{
    /// <summary>
    /// The collection of events that are executed per signal.
    /// </summary>
    [field: SerializeField, Tooltip("The collection of events that are executed per signal.")] 
    public SignalAssetEventPair[] SignalAssetEventPairs;

    /// <summary>
    /// A pairing of a signal asset with an event to be invoked.
    /// </summary>
    [Serializable]
    public class SignalAssetEventPair
    {
        /// <summary>
        /// The signal used as a filter for the timeline notifications.
        /// </summary>
        [field: SerializeField, Tooltip("The signal used as a filter for the timeline notifications.")] 
        public SignalAsset SignalAsset { get; set; }

        /// <summary>
        /// The actions to execute when the signal appears in the timeline.
        /// </summary>
        [field: SerializeField, Tooltip("The actions to execute when the signal appears in the timeline.")] 
        public ParameterizedEvent Events { get; set; }

        /// <summary>
        /// An event that will broadcast a <see cref="{T}"/> instance to listeners.
        /// </summary>
        [Serializable]
        public class ParameterizedEvent : UnityEvent<T> { }
    }

    /// <summary>
    /// Handles when a signal is read on the signal track.
    /// </summary>
    /// <param name="origin"> The instance the signal was read form.</param>
    /// <param name="notification"> The notification message.</param>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is ParameterizedSignalEmitter<T> emitter)
        {
            foreach (var signalPair in SignalAssetEventPairs)
            {
                if (ReferenceEquals(signalPair.SignalAsset, emitter.asset))
                    signalPair.Events.Invoke(emitter.Parameter);
            }
        }
    }
}
