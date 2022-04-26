using IKManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [field: SerializeField]
    private float MovementSpeed { get; set; }

    public Transform followTarget;

    public bool followingTarget;

    private void Start()
    {
        CustomIKManager.PRef = followTarget.localPosition;
    }

    private void Update()
    {
        if (followingTarget)
        {
            CustomIKManager.PRef = followTarget.localPosition;
        }
    }

    public void MoveTowards(Vector3 dir)
    {
        Vector3 newPos = followTarget.localPosition;
        newPos.x += dir.x * MovementSpeed;
        newPos.y += dir.y * MovementSpeed;
        newPos.z  += dir.z * MovementSpeed;
        followTarget.localPosition = newPos;
    }

}
