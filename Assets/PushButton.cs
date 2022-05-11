using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{

    private Rigidbody button;

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

    public UnityEvent OnButtonUp, OnButtonDown;

    private void Start()
    {
        button = GetComponentInChildren<Rigidbody>();
        original = button.transform.localPosition;
    }

    private void Update()
    {
        button.transform.localRotation = Quaternion.identity;
        // Clamp button positions
        button.transform.localPosition = new Vector3(original.x, Mathf.Clamp(button.transform.localPosition.y, original.y - travelDistance, original.y), original.z);

        // Check if button is up
        if (triggered && button.transform.localPosition.y >= original.y - toleranceY)
        {
            triggered = false;
            OnButtonUp.Invoke();
        }

        if (!triggered && button.transform.localPosition.y <= original.y - travelDistance + toleranceY)
        {
            triggered = true;
            OnButtonDown.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (transform.parent.parent == null)
        {
            button.velocity = Vector3.zero;
        }
        // Rise button if not on original 
        if (button.transform.localPosition.y < original.y)
        {
            button.AddForce(transform.up * travelSpeed * Time.deltaTime);
            return;
        }
        button.velocity = Vector3.zero;
    }
}
