using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class DelayOnboardingStart : MonoBehaviour
{
    [SerializeField] float _delay;
    [SerializeField] UnityEvent _onDelayedFirstPresence;
    [SerializeField] UnityEvent _onPresenceGained;
    [SerializeField] UnityEvent _onPresenceLost;

    [SerializeField] InputAction _userPresenceAction;

    private void Start()
    {
        Assert.IsTrue(_delay >= 0, "Delay cannot be negative.");

        _userPresenceAction.Enable();

        var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        // detect if user presence is supported
        if (headDevice != null)
        {
            bool presenceFeatureSupported = headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.userPresence, out var userPresentOnStart);

            if (!headDevice.isValid || !presenceFeatureSupported || !userPresentOnStart)
                _userPresenceAction.performed += HandleUserPresenceDelayedStart;
            else
                StartCoroutine(DelayInvokeCallbacks());
        }
        else
        {
            StartCoroutine(DelayInvokeCallbacks());
        }

        _userPresenceAction.started += (_) => _onPresenceGained?.Invoke();
        _userPresenceAction.canceled += (_) => _onPresenceLost?.Invoke();
    }

    private void HandleUserPresenceDelayedStart(InputAction.CallbackContext context)
    {
        var headsetIsOn = context.ReadValueAsButton();
        Debug.Log($"headset is on person: {headsetIsOn}");

        if (!headsetIsOn)
            return;

        _userPresenceAction.performed -= HandleUserPresenceDelayedStart;

        StartCoroutine(DelayInvokeCallbacks());
    }

    IEnumerator DelayInvokeCallbacks()
    {
        yield return new WaitForSeconds(_delay);
        _onDelayedFirstPresence.Invoke();
    }
}
