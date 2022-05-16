using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class ArticulationJointController : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 300.0f;

    private ArticulationBody articulation;

    // LIFE CYCLE
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate() 
    {
        if (LinearMovement.incremental)
        {
            if (rotationState != RotationDirection.None && Time.frameCount % LinearMovement.incrementInterval == 0)
            {
                Rotate();
            }
        }
        else
        {
            if (rotationState != RotationDirection.None )
            {
                Rotate();
            }
        }

    }

    private void Rotate()
    {
        float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;
        float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
        RotateTo(rotationGoal);
    }

    // MOVEMENT HELPERS
    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = articulation.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }



}
