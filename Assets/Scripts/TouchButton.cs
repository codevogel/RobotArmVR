using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private int phaseNumber;
    [SerializeField] private bool isPhaseChanger, isSkipButton;

    /// <summary>
    /// Activation of the buttons function if the player touches it
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftFinger")||other.CompareTag("RightFinger"))
        {
            //If the button is a button on the phase changing menu, change to the correct training phase
            if (isPhaseChanger)
            {
                TrainingScriptManager.Instance.ChangePhase(phaseNumber);
                return;
            }

            //If the button is on the warning message, deactivate the warning
            if (isSkipButton)
            {
                PhaseChanger.Instance.DeactivateWarning();
                PhaseChanger.Instance.UnlockAllButtons();
                return;
            }

            //If neither are true it is the exit button
            Application.Quit();
        }
    }
}
