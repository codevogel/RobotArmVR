using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotFollow : MonoBehaviour
{
    public RobotFollowTarget moveToTarget;
    public Transform lookAtTarget;

    public float moveSpeed;
    public float rotateSpeed;
    private bool reached = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        Vector3 moveDirection = moveToTarget.transform.position - transform.position;
        Vector3 lookDirection = lookAtTarget.transform.position - transform.position;
        Quaternion lookAtRotation = Quaternion.FromToRotation(this.transform.forward, lookDirection);

        if (moveDirection.magnitude >= moveToTarget.Radius)
        {
            Debug.Log("1");
            rb.MovePosition(this.transform.position + moveDirection * Time.deltaTime * moveSpeed);
        }
        else
        {
            if (!reached)
            {
                reached = true;
                StartCoroutine(LookAtTarget(lookAtRotation));
            }
        }
    }

    public IEnumerator LookAtTarget(Quaternion lookAtRotation)
    {
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, lerpValue);
            lerpValue += rotateSpeed;
            yield return new WaitForFixedUpdate();
        }

    }

}
