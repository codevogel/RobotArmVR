using IKManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [field: SerializeField]
    private float MovementSpeed { get; set; }

    [HideInInspector]
    public bool inBounds;

    [SerializeField]
    private Transform followTarget;

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
        if (inBounds)
        {
            Vector3 newPos = followTarget.localPosition;
            newPos.x += dir.x * MovementSpeed;
            newPos.y += dir.y * MovementSpeed;
            newPos.z += dir.z * MovementSpeed;
            followTarget.localPosition = newPos;
        }
        else
        {
            if ((followTarget.localPosition.x>0 && dir.x<0) || (followTarget.localPosition.x<0&&dir.x>0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.x += dir.x * MovementSpeed;
                followTarget.localPosition = newPos;
            }
            if ((followTarget.localPosition.y > 0 && dir.y < 0) || (followTarget.localPosition.y < 0 && dir.y > 0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.y += dir.y * MovementSpeed;
                followTarget.localPosition = newPos;
            }
            if ((followTarget.localPosition.z > 0 && dir.z < 0) || (followTarget.localPosition.z < 0 && dir.z > 0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.z += dir.z * MovementSpeed;
                followTarget.localPosition = newPos;
            }
        }
    }
}
