using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the UI of the flexpendant
/// </summary>
public class FlexpendantUIManager : MonoBehaviour
{
    private Transform propertyParent;
    private Transform positionParent;
    private Transform joystickDirectionParent;
    private Transform flangeTransform;

    private bool isLinear;

    private Axis[] axisValues= new Axis[7];

    private static string middleJoystickPicture1 = "JoystickAxisRight";
    private static string middleJoystickPicture2 = "JoystickAxisLeft";
    private static string rightJoystickPicture1 = "JoystickAxisTurnLeft";
    private static string rightJoystickPicture2 = "JoystickAxisTurnRight";

    private static int spacingNonLinear = 220;
    private static int spacingLinear = 210;
    private static int maxAxisRange = 360;

    private static Vector3 minPosition = new Vector3(16.1551f, -33.931f, -1.463f);
    private static Vector3 maxPosition = new Vector3(21.574f, -28.498f, 1.239f);

    private static int minRangeX = -1028, maxRangeX = 593, minRangeY= -1071,maxRangeY=2051, minRangeZ=-1260,maxRangeZ=2371;

    public static FlexpendantUIManager Instance { get { return _instance; } }
    private static FlexpendantUIManager _instance;

    private bool firstCheck=true;

    private void Awake()
    {
        _instance = this;

        for (int x = 0; x < 7; x++)
        {
            axisValues[x] = new Axis();
        }

        propertyParent = transform.GetChild(3).GetChild(2);
        joystickDirectionParent = transform.GetChild(2).GetChild(1);
        positionParent = transform.GetChild(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeAxisSet(true);
    }

    private void LateUpdate()
    {
        if (firstCheck)
        {
            transform.parent.gameObject.SetActive(false);
            firstCheck = false;
        }
    }

    /// <summary>
    /// Set the axis value on the UI at the beginning
    /// </summary>
    /// <param name="axis">The axis that needs to be set</param>
    public void SetAxis(ArticulationBody[] axis)
    {
        for (int x = 0; x < axis.Length; x++)
        {
            axisValues[x].axisRotation = ChangeAxis(x, axis[x].transform);
        }
        ChangePositionDisplay();
    }

    /// <summary>
    /// Update the axis value on the UI
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="axisTransform"></param>
    public void UpdateAxis(int axis, Transform axisTransform)
    {
        axisValues[axis].axisRotation = ChangeAxis(axis, axisTransform);
        ChangePositionDisplay();
    }

    /// <summary>
    /// Change the xyz positions in linear movement
    /// </summary>
    /// <param name="flange"></param>
    public void ChangeFlangePosition(Transform flange)
    {
        flangeTransform = flange;
        ChangePositionDisplay();
    }

    /// <summary>
    /// Change the specific axis value dependent on the input
    /// </summary>
    /// <param name="axis">The axis that needs to be change</param>
    /// <param name="axisTransform">The value of the axis which needs to be changed to</param>
    /// <returns></returns>
    public float ChangeAxis(int axis, Transform axisTransform)
    {
        float axisValue = 0;

        //Grab the specific rotational value from the transform
        switch (axis)
        {
            case 0:
                axisValue = axisTransform.localRotation.eulerAngles.y;
                break;

            case 1:
            case 2:
            case 4:
                axisValue = axisTransform.localRotation.eulerAngles.z;
                break;

            case 3:
            case 5:
                axisValue = axisTransform.rotation.x;
                break;
        }

        //Calculate if the value should move over to positive or negative 
        switch (axis)
        {
            case 0:
            case 1:
            case 2:
            case 4:
                if (axisValue >= 355 && axisValue <= 360
                && axisValues[axis].axisRotation >= 0 && axisValues[axis].axisRotation <= 5 && axisValues[axis].negative)
                {
                    axisValues[axis].negative = false;
                }

                if (axisValue >= 0 && axisValue <= 5
                    && axisValues[axis].axisRotation >= 355 && axisValues[axis].axisRotation <= 360 && !axisValues[axis].negative)
                {
                    axisValues[axis].negative = true;
                }
                break;

            case 3:
            case 5:
                if (axisValue>=0.8f &&axisValue<=1 &&axisValues[axis].axisRotation>=-1&&axisValues[axis].axisRotation<=0.8f&&!axisValues[axis].negative)
                {
                    axisValues[axis].negative = false;
                }

                if (axisValues[axis].axisRotation >= -1 && axisValue <= 0.8f && axisValues[axis].axisRotation >= 0.8f&& axisValues[axis].negative)
                {
                    axisValues[axis].negative = true;
                }
                break;
        }

        return axisValue;
    }

    /// <summary>
    /// Change a specific property displayed on the flexpendant
    /// </summary>
    /// <param name="property">The property to which it needs to change</param>
    /// <param name="option">The option to which it needs to change to</param>
    public void ChangeProperty(Properties property, int option)
    {
        switch (property)
        {
            case Properties.MOVEMENT_MODE:
                switch (option)
                {
                    case 0:
                        propertyParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Axis 1-3...";
                        break;
                    case 1:
                        propertyParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Axis 4-6...";
                        break;
                    case 2:
                        propertyParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Linear...";
                        break;
                }
                break;

            case Properties.COORDINATE_SYSTEM:
                switch (option)
                {
                    case 0:
                        propertyParent.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "World...";
                        break;
                }
                break;

            case Properties.TOOL:
                switch (option)
                {
                    case 0:
                        propertyParent.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Base...";
                        break;
                    case 1:
                        propertyParent.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Suction Cup Gripper...";
                        break;
                    case 2:
                        propertyParent.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Frasemotor...";
                        break;
                    case 3:
                        propertyParent.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "3D Printer...";
                        break;
                }
                break;

            case Properties.INCREMENT:
                switch (option)
                {
                    case 0:
                        propertyParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "None...";
                        break;
                    case 1:
                        propertyParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Small...";
                        break;
                    case 2:
                        propertyParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Medium...";
                        break;
                    case 3:
                        propertyParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Large...";
                        break;
                    case 4:
                        propertyParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "User...";
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Update the joystick indicator on the flexpendant
    /// </summary>
    /// <param name="linear">Check whether the robot is in linear mode</param>
    public void ChangeDirectionDisplay(bool linear)
    {
        isLinear = linear;
        TextMeshProUGUI text = joystickDirectionParent.GetChild(0).GetComponent<TextMeshProUGUI>();

        //Change the joystick indicator to the linear-mode directions
        if (!isLinear)
        {
            text.characterSpacing = spacingNonLinear;
            ChangeAxisSet(true);
            return;
        }

        //Change the joystick indicator to the axis-mode directions
        text.text = "XYZ";
        text.characterSpacing = spacingLinear;
        joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture1);
        joystickDirectionParent.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + rightJoystickPicture1);
        ChangeProperty(Properties.MOVEMENT_MODE, 2);
    }

    /// <summary>
    /// Change the joystick indicator between both axis indication possiblities
    /// </summary>
    /// <param name="axisSetOne">Check whether the axis mode is on set one or two</param>
    public void ChangeAxisSet(bool axisSetOne)
    {
        if (!isLinear)
        {
            TextMeshProUGUI text = joystickDirectionParent.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (axisSetOne)
            {
                text.text = "213";
                joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture1);
                joystickDirectionParent.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + rightJoystickPicture2);
                ChangeProperty(Properties.MOVEMENT_MODE, 0);
                return;
            }
            text.text = "546";
            joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture2);
            joystickDirectionParent.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + rightJoystickPicture1);
            ChangeProperty(Properties.MOVEMENT_MODE, 1);
        }
    }

    /// <summary>
    /// Update the displayed axis values
    /// </summary>
    private void ChangePositionDisplay()
    {
        positionParent.GetChild(2).GetComponent<TextMeshProUGUI>().text = null;
        TextMeshProUGUI textToChange = positionParent.GetChild(2).GetComponent<TextMeshProUGUI>();
        string message = null;

        if (!isLinear)
        {
            positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1: \n2: \n3: \n4: \n5: \n6: ";

            //Update each individual axis in axis mode and show if its positive or negative
            for (int x = 0; x < axisValues.Length - 1; x++)
            {
                switch (x)
                {
                    case 0:
                    case 2:
                    case 4:
                        //Display the value in negative
                        if (axisValues[x].negative)
                        {
                            message += "-"+Math.Round(axisValues[x].axisRotation, 2) + "°\n";
                            break;
                        }
                        message += Math.Round(maxAxisRange - axisValues[x].axisRotation, 2) + "°\n";
                        break;
                    case 1:
                        if (axisValues[x].negative)
                        {
                            message += Math.Round(axisValues[x].axisRotation, 2) + "°\n";
                            break;
                        }
                        message += "-" + Math.Round(maxAxisRange - axisValues[x].axisRotation, 2) + "°\n";
                        break;
                    case 3:
                    case 5:
                        float rotation = (axisValues[x].axisRotation / 2) + 0.5f;
                        float rotationDisplay= Mathf.Lerp(-maxAxisRange, maxAxisRange, rotation);
                        message += Math.Round(rotationDisplay, 2) + "°\n";
                        break;
                }
            }
            textToChange.text = message;
            return;
        }
        positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "X: \nY: \nZ:"; //"nq1: \nq2: \nq3: \nq4: "

        //Update the XYZ coordinates of the end effector of the robot
        for (int x = 0; x < axisValues.Length; x++)
        {
            switch (x)
            {
                case 0:
                    float percentileX= CalculatePercentile(minPosition.x, maxPosition.x, flangeTransform.position.x);
                    //Start at max as the percentage should be inverted
                    message += Math.Round(Mathf.Lerp(maxRangeX, minRangeX, percentileX),2) + " mm\n";
                    break;
                case 1:
                    float percentileY = CalculatePercentile(minPosition.y, maxPosition.y, flangeTransform.position.z);
                    message += Math.Round(Mathf.Lerp(maxRangeY, minRangeY, percentileY), 2) + " mm\n";
                    break;
                case 2:
                    float percentileZ = CalculatePercentile(minPosition.z, maxPosition.z, flangeTransform.position.y);
                    message += Math.Round(Mathf.Lerp(minRangeZ, maxRangeZ, percentileZ), 2) + " mm\n";
                    break;
            }
        }
        textToChange.text = message;
    }

    /// <summary>
    /// Calculate which percent the X/Y/Z coordinate is between its minimum and maximum value
    /// </summary>
    /// <param name="min">Minimum value of coordinate</param>
    /// <param name="max">Maximum value of coordinate</param>
    /// <param name="value">Value of coordinate</param>
    /// <returns></returns>
    private float CalculatePercentile(float min, float max, float value)
    {
        float basePercentage = Mathf.InverseLerp(min,max,value);
        return basePercentage;
    }

    public enum Properties
    {
        MOVEMENT_MODE,
        COORDINATE_SYSTEM,
        TOOL,
        INCREMENT
    }

    /// <summary>
    /// Holds the value of an axis and if it is negative or not
    /// </summary>
    public class Axis
    {
        public float axisRotation;
        public bool negative;

        public Axis()
        {
            axisRotation = 0;
            negative = false;
        }
    }
}