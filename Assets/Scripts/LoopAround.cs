using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAround : MonoBehaviour
{
    public float rotateSpeed;
    public bool lookAt;
    public Transform lookAtTarget; 
    [SerializeField]
    private float amplitude;
    private float originalY;
    [SerializeField]
    private float offset;

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        this.transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);

        Vector3 position = transform.position;
        position.y = originalY + Mathf.Sin(Time.time * 2 + offset) * amplitude;
        transform.position = position;
        if (lookAt)
            this.transform.LookAt(lookAtTarget);
    }
}
