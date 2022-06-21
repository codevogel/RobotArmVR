using IKManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public Transform[] followTarget;

    public bool followingTarget;

    [SerializeField]
    private CustomIKManager[] customIKManager;

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

    [HideInInspector]
    public int currentRobot=0;

    float previousAngle;

    private void Start()
    {
        customIKManager[currentRobot].PRef = followTarget[currentRobot].localPosition;
        previousAngle = 0;
    }

    private void Update()
    {
        if (followingTarget)
        {
            customIKManager[currentRobot].PRef = followTarget[currentRobot].localPosition;
        }
    }

    public void ChangeRobot(int robotNumber)
    {
        currentRobot = robotNumber;

        RecalculatePreviousAngle();
    }

    public void ChangeMode()
    {
        incremental = !incremental;
        FlexpendantUIManager.Instance.ChangeProperty(FlexpendantUIManager.Properties.INCREMENT, incremental ? 3 : 0);
    }

    /// <summary>
    /// Move end effector towards dir
    /// </summary>
    /// <param name="dir">the direction to move to</param>
    public void MoveTowards(Vector3 dir)
    {
        if (incremental)
        {
            // Incremental movement
            if (Time.frameCount % incrementInterval == 0)
            {
                _MoveTowards(dir, incrementSize);
            }
        }
        else
        {
            // Normal movement
            _MoveTowards(dir, MovementSpeed);
        }
    }

    /// <summary>
    /// Recalculates the previous angle recorded used for angle correction.
    /// </summary>
    public void RecalculatePreviousAngle()
    {
        var position = followTarget[currentRobot].localPosition;
        var robot = customIKManager[currentRobot].Joint[0].transform.parent;
        var rotationAxis = new Vector3(position.x, 0, position.z);

        previousAngle = Vector3.SignedAngle(rotationAxis, Vector3.left, Vector3.up);
    }

    /// <summary>
    /// Move end effector towards dir by distance
    /// </summary>
    /// <param name="dir">the direction to move to</param>
    /// <param name="distance">the distance to move</param>
    private void _MoveTowards(Vector3 dir, float distance)
    {
        Vector3 newPos = followTarget[currentRobot].localPosition;
        
        if (customIKManager[currentRobot].inBounds)
        {
            newPos += dir * distance;
            CorrectForBaseJoint(ref newPos);
            followTarget[currentRobot].localPosition = newPos;
            customIKManager[currentRobot].UpdateUI();
        }
        else // only allow directions that make the robot go in bounds again
        {
            if ((followTarget[currentRobot].localPosition.x > 0 && dir.x < 0) || (followTarget[currentRobot].localPosition.x < 0 && dir.x > 0))
            {
                newPos.x += dir.x * distance;
                CorrectForBaseJoint(ref newPos);
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
            if ((followTarget[currentRobot].localPosition.y > 0 && dir.y < 0) || (followTarget[currentRobot].localPosition.y < 0 && dir.y > 0))
            {
                newPos.y += dir.y * distance;
                CorrectForBaseJoint(ref newPos);
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
            if ((followTarget[currentRobot].localPosition.z > 0 && dir.z < 0) || (followTarget[currentRobot].localPosition.z < 0 && dir.z > 0))
            {
                newPos.z += dir.z * distance;
                CorrectForBaseJoint(ref newPos);
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
        }
    }

    /// <summary>
    /// Corrects the position to not surpass the limits of the base joint of the robot.
    /// </summary>
    /// <param name="position"> The current position to correct.</param>
    private void CorrectForBaseJoint(ref Vector3 position)
    {
        var robot = customIKManager[currentRobot].Joint[0].transform.parent;
        var rotationAxis = new Vector3(position.x, 0, position.z);

        Debug.Log(robot.forward);

        // check if the follow target wrapped around
        var currentAngle = Vector3.SignedAngle(rotationAxis.normalized, Vector3.left, Vector3.up);

        if (!((previousAngle * currentAngle) >= 0) && Mathf.Abs(currentAngle) > 90f)
        {
            position = Quaternion.Euler(0, currentAngle - previousAngle, 0) * position;
        }
        else
        {
            previousAngle = currentAngle;
        }
    }
}