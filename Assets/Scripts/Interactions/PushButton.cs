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

    public bool frozen;

    public UnityEvent OnButtonUp, OnButtonDown;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        original = rb.transform.localPosition;
    }

    private void Update()
    {
        rb.transform.localRotation = Quaternion.identity;
        // Clamp button positions
        rb.transform.localPosition = new Vector3(original.x, Mathf.Clamp(rb.transform.localPosition.y, original.y - travelDistance, original.y), original.z);

        // Check if button is up
        if (triggered && rb.transform.localPosition.y >= original.y - toleranceY)
        {
            triggered = false;
            OnButtonUp.Invoke();
        }

        if (!triggered && rb.transform.localPosition.y <= original.y - travelDistance + toleranceY)
        {
            triggered = true;
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
        rb.velocity = Vector3.zero;
    }

    public void Reset()
    {
        rb.transform.localPosition = original;
        triggered = false;
    }

    public void FreezeButton(bool freeze)
    {
        frozen = freeze;
        rb.isKinematic = freeze;
    }
}
