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

    private void Start()
    {
        customIKManager[currentRobot].PRef = followTarget[currentRobot].localPosition;
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
    /// Move end effector towards dir by distance
    /// </summary>
    /// <param name="dir">the direction to move to</param>
    /// <param name="distance">the distance to move</param>
    private void _MoveTowards(Vector3 dir, float distance)
    {
        if (customIKManager[currentRobot].inBounds)
        {
            Vector3 newPos = followTarget[currentRobot].localPosition;
            newPos += dir * distance;
            followTarget[currentRobot].localPosition = newPos;
            customIKManager[currentRobot].UpdateUI();
        }
        else // only allow directions that make the robot go in bounds again
        {
            if ((followTarget[currentRobot].localPosition.x > 0 && dir.x < 0) || (followTarget[currentRobot].localPosition.x < 0 && dir.x > 0))
            {
                Vector3 newPos = followTarget[currentRobot].localPosition;
                newPos.x += dir.x * distance;
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
            if ((followTarget[currentRobot].localPosition.y > 0 && dir.y < 0) || (followTarget[currentRobot].localPosition.y < 0 && dir.y > 0))
            {
                Vector3 newPos = followTarget[currentRobot].localPosition;
                newPos.y += dir.y * distance;
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
            if ((followTarget[currentRobot].localPosition.z > 0 && dir.z < 0) || (followTarget[currentRobot].localPosition.z < 0 && dir.z > 0))
            {
                Vector3 newPos = followTarget[currentRobot].localPosition;
                newPos.z += dir.z * distance;
                followTarget[currentRobot].localPosition = newPos;
                customIKManager[currentRobot].UpdateUI();
            }
        }
    }
}