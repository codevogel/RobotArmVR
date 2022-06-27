using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;

/// <summary>
/// Check if the user is wearing the headset
/// </summary>
public class PresenceCheck : MonoBehaviour
{
    [SerializeField] UnityEvent _onPresenceGained;
    [SerializeField] UnityEvent _onPresenceLost;

    [SerializeField] InputAction _userPresenceAction;

    private void Start()
    {
        _userPresenceAction.Enable();

        _userPresenceAction.started += (_) => _onPresenceGained?.Invoke();
        _userPresenceAction.canceled += (_) => _onPresenceLost?.Invoke();
    }
}
