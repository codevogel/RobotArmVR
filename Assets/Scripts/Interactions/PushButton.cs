using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    [Tooltip("The distance this button can travel")]
    [field: SerializeField]
    private float travelDistance;

    [Tooltip("The speed at which the button travels upwards")]
    [field: SerializeField]
    private float travelSpeed;


    [Tooltip("Tolerance distance for triggering button down/up events")]
    [field: SerializeField]
    private float toleranceY = 0.05f;


    private Vector3 original;
    private bool triggered;
    private bool buttonEnabled;

    public bool frozen;

    public UnityEvent OnButtonUp, OnButtonDown;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        original = rb.transform.localPosition;
    }

    private void Update()
    {
        // Prevent rotation
        rb.transform.localRotation = Quaternion.identity;
        // Clamp button positions
        rb.transform.localPosition = new Vector3(original.x, Mathf.Clamp(rb.transform.localPosition.y, original.y - travelDistance, original.y), original.z);

        // Check if button is up
        if (triggered && rb.transform.localPosition.y >= original.y - toleranceY && buttonEnabled)
        {
            buttonEnabled = false;
            triggered = false;
            OnButtonUp.Invoke();
        }
        // Check if button is down
        if (!triggered && rb.transform.localPosition.y <= original.y - travelDistance + toleranceY &&!buttonEnabled)
        {
            triggered = true;
            buttonEnabled = true;
            OnButtonDown.Invoke();
        }
    }

    private void FixedUpdate()
    {
        // Rise button if not on original 
        if (rb.transform.localPosition.y < original.y)
        {
            rb.AddForce(transform.up * travelSpeed * Time.deltaTime);
            return;
        }
        // Otherwise make sure button doesn't move.
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Resets this button
    /// </summary>
    public void Reset()
    {
        rb.transform.localPosition = original;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        triggered = false;
    }

    /// <summary>
    /// Freezes this button based on the bool
    /// </summary>
    /// <param name="freeze">whether to freeze this button</param>
    public void FreezeButton(bool freeze)
    {
        frozen = freeze;
        rb.isKinematic = freeze;
    }
}
