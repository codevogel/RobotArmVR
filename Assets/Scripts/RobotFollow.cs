using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotFollow : MonoBehaviour
{
    private MoveState moveState = MoveState.STANDBY;

    public RobotFollowTarget moveToTarget;
    public Transform lookAtTarget;

    public float moveSpeed;
    public float rotateSpeed;

    private Quaternion initRotation;
    private Quaternion lookAtRotation;
    private float lookAtLerpValue = 0;
    private float moveToLerpValue = 0;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (moveState)
        {
            case MoveState.STANDBY:
                AttemptToStartMoving();
                LookTowardsTarget();
                break;
            case MoveState.MOVING:
                MoveTowardsTarget();
                LookTowardsTarget();
                break;
            default:
                break;
        }
    }
    private void AttemptToStartMoving()
    {
        Vector3 moveDirection = moveToTarget.transform.position - transform.position;
        if (moveDirection.magnitude > moveToTarget.Radius)
        {
            SwitchState(MoveState.MOVING);
        }
    }
    private void InitLookAt(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        initRotation = transform.rotation;
        lookAtRotation = Quaternion.FromToRotation(this.transform.forward, lookDirection);
        lookAtLerpValue = 0;
    }

    private void LookTowardsTarget()
    {
        this.transform.rotation = Quaternion.Slerp(initRotation, lookAtRotation, lookAtLerpValue);
        lookAtLerpValue += rotateSpeed;
    }

    private void MoveTowardsTarget()
    {
        rb.MovePosition(this.transform.position + ((moveToTarget.transform.position - this.transform.position) * moveSpeed * Time.deltaTime));
        if ((moveToTarget.transform.position - this.transform.position).magnitude < moveToTarget.Radius)
        {
            SwitchState(MoveState.STANDBY);
        }
    }


    private void SwitchState(MoveState state)
    {
        moveState = state;
        OnSwitchState(state);
    }

    private void OnSwitchState(MoveState state)
    {

        switch (state)
        {
            case MoveState.STANDBY:
                InitLookAt(lookAtTarget.transform);
                break;
            case MoveState.MOVING:
                InitLookAt(moveToTarget.transform);
                break;
            default:
                break;
        }
    }

}
