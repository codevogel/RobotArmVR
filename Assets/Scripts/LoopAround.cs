using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the tiefighter and it's LookAtTarget
/// </summary>
public class LoopAround : MonoBehaviour
{
    [Tooltip("Speed to lap around the ship")]
    public float rotateSpeed;
    [Tooltip("Speed at which the tiefighter flies up/down")]
    public float phaseSpeed = 2f;

    [Tooltip("Set to true for the tiefighter")]
    public bool lookAt;

    public Transform lookAtTarget;
    
    [Tooltip("Determines how far up/down the tiefighter flies")]
    [SerializeField]
    private float amplitude;
    [Tooltip("Offset in sinus time")]
    [SerializeField]
    private float offset;

    private float originalY;

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        // Rotate around origin
        this.transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);

        // Set y position
        Vector3 position = transform.position;
        position.y = originalY + Mathf.Sin(Time.time * 2 + offset) * amplitude;
        transform.position = position;

        // Look at target
        if (lookAt)
            this.transform.LookAt(lookAtTarget);
    }
}
