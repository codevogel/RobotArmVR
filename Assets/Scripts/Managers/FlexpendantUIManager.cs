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

    private float[] allAxisValues = new float[7];

    private static string middleJoystickPicture1 = "JoystickAxisLeft";
    private static string middleJoystickPicture2 = "JoystickAxisRight";

    private static int spacingNonLinear = 220;
    private static int spacingLinear = 210;

    public static FlexpendantUIManager Instance { get { return _instance; } }
    private static FlexpendantUIManager _instance;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        propertyParent = transform.GetChild(3).GetChild(2);
        joystickDirectionParent = transform.GetChild(2).GetChild(1);
        positionParent = transform.GetChild(1);
        ChangeProperty(Properties.MOVEMENT_MODE, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAxis(Transform[] axis)
    {
        for(int x=0; x<axis.Length;x++)
        {
            allAxisValues[x] = ChangeAxis(x, axis[x]);
        }
        ChangePositionDisplay();
    }

    public void UpdateAxis(int axis, Transform axisTransform)
    {
        allAxisValues[axis]=ChangeAxis(axis, axisTransform);
        ChangePositionDisplay();
    }

    public float ChangeAxis(int axis, Transform axisTransform)
    {
        float axisValue=0;

        switch (axis)
        {
            case 0:
                axisValue = axisTransform.localRotation.eulerAngles.x;
                break;
            case 1:
                axisValue = axisTransform.localRotation.eulerAngles.z;
                break;
            case 2:
                axisValue = axisTransform.localRotation.eulerAngles.x;
                break;
            case 3:
                axisValue = axisTransform.localRotation.eulerAngles.y;
                break;
            case 4:
                axisValue = axisTransform.localRotation.eulerAngles.x;
                break;
            case 5:
                axisValue = axisTransform.localRotation.eulerAngles.y;
                break;
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
            joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture1);
            return;
        }
        text.text = "XYZ";
        text.characterSpacing = spacingLinear;
        joystickDirectionParent.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + middleJoystickPicture2);
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
                ChangeProperty(Properties.MOVEMENT_MODE, 0);
                return;
            }
            text.text = "546";
            ChangeProperty(Properties.MOVEMENT_MODE, 1);
        }
    }

    private void ChangePositionDisplay()
    {
        positionParent.GetChild(2).GetComponent<TextMeshProUGUI>().text = null;
        if (!isLinear)
        {
            positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1: \n2: \n3: \n4: \n5: \n6: ";
            positionParent.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            Math.Round(allAxisValues[0], 2).ToString() + "°\n" +
            Math.Round(allAxisValues[1], 2).ToString() + "°\n" +
            Math.Round(allAxisValues[2], 2).ToString() + "°\n" +
            Math.Round(allAxisValues[3], 2).ToString() + "°\n" +
            Math.Round(allAxisValues[4], 2).ToString() + "°\n" +
            Math.Round(allAxisValues[5], 2).ToString() + "°";
            return;
        }
        positionParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "X: \nY: \nZ: \nq1: \nq2: \nq3: \nq4: ";
        positionParent.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            Math.Round(allAxisValues[0], 2).ToString() + "\n" +
            Math.Round(allAxisValues[1], 2).ToString() + "\n" +
            Math.Round(allAxisValues[2], 2).ToString() + "\n" +
            Math.Round(allAxisValues[3], 2).ToString() + "\n" +
            Math.Round(allAxisValues[4], 2).ToString() + "\n" +
            Math.Round(allAxisValues[5], 2).ToString() + "\n" +
            Math.Round(allAxisValues[6], 2).ToString();
    }

    public enum Properties
    {
        MOVEMENT_MODE,
        COORDINATE_SYSTEM,
        TOOL,
        INCREMENT
    }
}
