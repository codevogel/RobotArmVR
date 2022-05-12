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


    private float originalY;
    private bool triggered;

    public UnityEvent OnButtonUp, OnButtonDown;

    private void Start()
    {
        button = GetComponentInChildren<Rigidbody>();
        originalY = button.transform.localPosition.y;
    }

    private void Update()
    {
        // Clamp button positions
        button.transform.localPosition = new Vector3(button.transform.localPosition.x, Mathf.Clamp(button.transform.localPosition.y, originalY - travelDistance, originalY), button.transform.localPosition.z);
    
        // Check if button is up
        if (triggered && button.transform.localPosition.y >= originalY - toleranceY)
        {
            triggered = false;
            OnButtonUp.Invoke();
        }

        if (!triggered && button.transform.localPosition.y <= originalY - travelDistance + toleranceY)
        {
            triggered = true;
            OnButtonDown.Invoke();
        }
    }

    private void FixedUpdate()
    {
        // Rise button if not on original 
        button.velocity = button.transform.localPosition.y < originalY ? Vector3.up * travelSpeed * Time.deltaTime : Vector3.zero;
    }

    public virtual void ButtonDown()
    {
        GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
    }
    public virtual void ButtonUp()
    {
        GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
    }
}
