using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves a character towards a target and looks at it.
/// </summary>
public class TargetFollow : MonoBehaviour
{

    [Header("Targets")]
    public RobotFollowTarget moveToTarget;
    public Transform lookAtTarget;

    [Header("Speeds")]
    public float moveSpeed;
    public float rotateSpeed;

    private MoveState moveState = MoveState.STANDBY;
    private Quaternion initRotation;
    private float lookAtLerpValue = 0;
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
                LookTowardsTarget(lookAtTarget);
                break;
            case MoveState.MOVING:
                MoveTowardsTarget();
                LookTowardsTarget(moveToTarget.transform);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Checks whether the character has to start moving and if so, starts moving.
    /// </summary>
    private void AttemptToStartMoving()
    {
        Vector3 moveDirection = moveToTarget.transform.position - transform.position;
        if (moveDirection.magnitude > moveToTarget.Radius)
        {
            SwitchState(MoveState.MOVING);
        }
    }

    /// <summary>
    /// Initializes the Look At action.
    /// </summary>
    /// <param name="target">The target to look at</param>
    private void InitLookAt(Transform target)
    {
        initRotation = transform.rotation;
        lookAtLerpValue = 0;
    }

    /// <summary>
    /// Lerps look rotation towards target.
    /// </summary>
    /// <param name="target">The target to look at</param>
    private void LookTowardsTarget(Transform target)
    {
        this.transform.rotation = Quaternion.Slerp(initRotation, Quaternion.LookRotation(target.position - transform.position), lookAtLerpValue);
        lookAtLerpValue += rotateSpeed;
    }

    /// <summary>
    /// Moves character towards moveToTarget by moveSpeed.
    /// </summary>
    private void MoveTowardsTarget()
    {
        rb.MovePosition(this.transform.position + ((moveToTarget.transform.position - this.transform.position) * moveSpeed * Time.deltaTime));
        if ((moveToTarget.transform.position - this.transform.position).magnitude < moveToTarget.Radius)
        {
            SwitchState(MoveState.STANDBY);
        }
    }

    /// <summary>
    /// Switches the current state.
    /// </summary>
    /// <param name="state"></param>
    private void SwitchState(MoveState state)
    {

        moveState = state;
        OnSwitchState(state);
    }

    /// <summary>
    /// Called on state switch.
    /// </summary>
    /// <param name="state">The state that was switched to..</param>
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
