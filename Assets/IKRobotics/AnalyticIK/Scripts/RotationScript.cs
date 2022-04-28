using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Text;

namespace AnalyticIK
{
public class RotationScript : MonoBehaviour
{
    public Transform target;
    public Vector3 direction;
    public int axisNum;
    private Vector3 targetPos;
    private float[] data = new float[6];
    private float[] angle = new float[6];
    
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            angle[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
			data[0] = (float)(AnalyticIK.CalcIK.theta[0]) * 180.0f / 3.14f;
			data[1] = (float)(AnalyticIK.CalcIK.theta[1]) * 180.0f / 3.14f;
			data[2] = (float)(AnalyticIK.CalcIK.theta[2]) * 180.0f / 3.14f - 90.0f;
			data[3] = (float)(AnalyticIK.CalcIK.theta[3]) * 180.0f / 3.14f;
			data[4] = (float)(AnalyticIK.CalcIK.theta[4]) * 180.0f / 3.14f;
			data[5] = (float)(AnalyticIK.CalcIK.theta[5]) * 180.0f / 3.14f;
        if (angle[axisNum] != data[axisNum])
        {
            targetPos = target.position;
            transform.RotateAround(targetPos, target.up, data[axisNum]-angle[axisNum]);
            angle[axisNum] = data[axisNum];
        }
    }
}
}