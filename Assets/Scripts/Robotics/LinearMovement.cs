using IKManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{

    [HideInInspector]
    public bool inBounds;

    public Transform followTarget;

    public bool followingTarget;

    [SerializeField]
    private CustomIKManager customIKManager;

    float previousAngle;
    Transform robot;

    #region Continuous
    [field: SerializeField]
    private float MovementSpeed { get; set; }
    #endregion

    #region Increment
    public static bool incremental = false;
    public static int incrementInterval = 10;
    [field: SerializeField]
    private float incrementSize;
    #endregion

    private void Start()
    {
        CustomIKManager.PRef = followTarget.localPosition;
        previousAngle = 0;
        robot = customIKManager.Joint[0].transform.parent;
    }

    private void Update()
    {
        if (followingTarget)
        {
            var currentPosition = followTarget.localPosition;
            var rotationAxis = new Vector3(currentPosition.x, 0, currentPosition.z);

            // check if the follow target wrapped around
            Debug.Log(Vector3.SignedAngle(rotationAxis, robot.right, Vector3.up));

            // check if same sign
            // (num1 ^ num2) >= 0

            CustomIKManager.PRef = followTarget.localPosition;
        }
    }

    public void ChangeMode()
    {
        incremental = !incremental;
        FlexpendantUIManager.Instance.ChangeProperty(FlexpendantUIManager.Properties.INCREMENT, incremental ? 3 : 0);
    }

    public void MoveTowards(Vector3 dir)
    {
        if (incremental)
        {
            if (Time.frameCount % incrementInterval == 0)
            {
                _MoveTowards(dir, incrementSize);
            }
        }
        else
        {
            _MoveTowards(dir, MovementSpeed);
        }
    }

    private void _MoveTowards(Vector3 dir, float distance)
    {
        if (inBounds)
        {
            Vector3 newPos = followTarget.localPosition;
            newPos += dir * distance;
            followTarget.localPosition = newPos;
            customIKManager.UpdateUI();
        }
        else // only allow directions that make the robot go in bounds again
        {
            if ((followTarget.localPosition.x > 0 && dir.x < 0) || (followTarget.localPosition.x < 0 && dir.x > 0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.x += dir.x * distance;
                followTarget.localPosition = newPos;
                customIKManager.UpdateUI();
            }
            if ((followTarget.localPosition.y > 0 && dir.y < 0) || (followTarget.localPosition.y < 0 && dir.y > 0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.y += dir.y * distance;
                followTarget.localPosition = newPos;
                customIKManager.UpdateUI();
            }
            if ((followTarget.localPosition.z > 0 && dir.z < 0) || (followTarget.localPosition.z < 0 && dir.z > 0))
            {
                Vector3 newPos = followTarget.localPosition;
                newPos.z += dir.z * distance;
                followTarget.localPosition = newPos;
                customIKManager.UpdateUI();
            }
        }
    }
}
