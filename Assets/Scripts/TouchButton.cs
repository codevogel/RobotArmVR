using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private int phaseNumber;
    [SerializeField] private ButtonFunction buttonFunction;

    [SerializeField] private RobotArmController robot;

    [HideInInspector]
    public bool locked;

    /// <summary>
    /// Activation of the buttons function if the player touches it
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("LeftFinger") || other.CompareTag("RightFinger")) && !locked)
        {
            PhaseChanger.Instance.LockPushingButtons();

            switch (buttonFunction)
            {
                //If the button is a button on the phase changing menu, change to the correct training phase
                case ButtonFunction.PHASECHANGER:
                    TrainingScriptManager.Instance.ChangePhase(phaseNumber);
                    break;

                //If the button is on the warning message, deactivate the warning
                case ButtonFunction.SKIPBUTTON:
                    PhaseChanger.Instance.DeactivateWarning();
                    PhaseChanger.Instance.UnlockAllButtons();
                    break;

                //If the button is the exit button
                case ButtonFunction.EXITGAMEBUTTON:
                    Application.Quit();
                    break;
                case ButtonFunction.DEBUG:
                    robot.ChangeRobot(1);
                    break;
            }
        }
    }

    private enum ButtonFunction
    {
        PHASECHANGER,
        SKIPBUTTON,
        EXITGAMEBUTTON,
        DEBUG
    }
}
