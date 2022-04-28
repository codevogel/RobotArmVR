using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlexpendantUIManager : MonoBehaviour
{
    private Transform propertyParent;
    private Transform positionParent;
    private Transform joystickDirectionParent;

    private bool isLinear;

    private Axis[] axisValues= new Axis[7];

    private static string middleJoystickPicture1 = "JoystickAxisRight";
    private static string middleJoystickPicture2 = "JoystickAxisLeft";
    private static string rightJoystickPicture1 = "JoystickAxisTurnLeft";
    private static string rightJoystickPicture2 = "JoystickAxisTurnRight";

    private static int spacingNonLinear = 220;
    private static int spacingLinear = 210;

    public static FlexpendantUIManager Instance { get { return _instance; } }
    private static FlexpendantUIManager _instance;

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

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAxis(ArticulationBody[] axis)
    {
        for (int x = 0; x < axis.Length; x++)
        {
            axisValues[x].axisRotation = ChangeAxis(x, axis[x].transform);
        }
        ChangePositionDisplay();
    }

    public void UpdateAxis(int axis, Transform axisTransform)
    {
        axisValues[axis].axisRotation = ChangeAxis(axis, axisTransform);
        ChangePositionDisplay();
    }

    public float ChangeAxis(int axis, Transform axisTransform)
    {
        float axisValue = 0;

        switch (axis)
        {
            case 0:
            case 4:
                axisValue = axisTransform.localRotation.eulerAngles.x;
                break;
            case 1:
                axisValue = axisTransform.localRotation.eulerAngles.z;
                break;
            case 2:
                axisValue = axisTransform.localRotation.eulerAngles.x;
                break;
            case 3:
            case 5:
                axisValue = axisTransform.localRotation.eulerAngles.y;
                break;
        }

        if (axisValue>=355 && axisValue<=360 
            &&axisValues[axis].axisRotation>=0 && axisValues[axis].axisRotation<=5 && !axisValues[axis].negative)
        {
            axisValues[axis].negative = true;
        }

        if (axisValue>0 && axisValue <=5 
            && axisValues[axis].axisRotation >= 355 && axisValues[axis].axisRotation <= 360 && axisValues[axis].negative)
        {
            axisValues[axis].negative = false;
        }

        return axisValue;
    }

    public void ChangeProperty(Properties property, int option)
    {
        switch (property)
        {
            case Properties.MOVEMENT_MODE:
                switch (option)
                {
                    case 0:
                        propertyParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Axis 1 - 3...";
                        break;
                    case 1:
                        propertyParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Axis 4 - 6...";
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

    public void ChangeDirectionDisplay(bool linear)
    {
        isLinear = linear;
        TextMeshProUGUI text = joystickDirectionParent.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (!isLinear)
        {
            text.characterSpacing = spacingNonLinear;
            ChangeAxisSet(true);
            return;
        }
        text.text = "XYZ";
        text.characterSpacing = spacingLinear;
        joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture1);
        joystickDirectionParent.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + rightJoystickPicture1);
        ChangeProperty(Properties.MOVEMENT_MODE, 2);
    }

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

    private void ChangePositionDisplay()
    {
        positionParent.GetChild(2).GetComponent<TextMeshProUGUI>().text = null;
        TextMeshProUGUI textToChange = positionParent.GetChild(2).GetComponent<TextMeshProUGUI>();
        string message = null;

        if (!isLinear)
        {
            positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1: \n2: \n3: \n4: \n5: \n6: ";

            for (int x = 0; x < axisValues.Length - 1; x++)
            {
                
                switch (x)
                {
                    case 0:
                        if (axisValues[x].negative)
                        {
                            message += Math.Round((360 - axisValues[x].axisRotation)-360, 2).ToString() + "°\n";
                            break;
                        }
                        message += Math.Round(360 - axisValues[x].axisRotation, 2).ToString() + "°\n";
                        break;
                    case 1:
                        if (axisValues[x].negative)
                        {
                            message += Math.Round(axisValues[x].axisRotation-360, 2).ToString() + "°\n";
                            break;
                        }
                        message += Math.Round(axisValues[x].axisRotation, 2).ToString() + "°\n";
                        break;
                    case 2:
                        if (axisValues[x].negative)
                        {
                            message += Math.Round(axisValues[x].axisRotation - 360, 2).ToString() + "°\n";
                            break;
                        }
                        message += Math.Round(axisValues[x].axisRotation, 2).ToString() + "°\n";
                        break;
                    case 3:
                    case 4:
                    case 5:
                        if (axisValues[x].negative)
                        {
                            message += Math.Round((360 - axisValues[x].axisRotation)-360, 2).ToString() + "°\n";
                            break;
                        }
                        message += Math.Round(360 - axisValues[x].axisRotation, 2).ToString() + "°\n";
                        break;
                }
            }
            textToChange.text = message;
            return;
        }
        positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "X: \nY: \nZ: \nq1: \nq2: \nq3: \nq4: ";

        for (int x = 0; x < axisValues.Length; x++)
        {
            switch (x)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    message += Math.Round(axisValues[x].axisRotation, 2).ToString() + "\n";
                    break;
            }
        }
        textToChange.text = message;
    }

    public enum Properties
    {
        MOVEMENT_MODE,
        COORDINATE_SYSTEM,
        TOOL,
        INCREMENT
    }

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

