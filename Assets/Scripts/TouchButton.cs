using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private int phaseNumber;
    [SerializeField] private int subPhaseNumber;
    [SerializeField] private bool isPhaseChanger, isSkipButton;
    private PhaseChanger phaseChanger;

    private void Start()
    {
        if (isSkipButton)
        {
            phaseChanger = transform.GetComponentInParent<PhaseChanger>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftFinger")||other.CompareTag("RightFinger"))
        {
            if (isPhaseChanger)
            {
                TrainingScriptManager.Instance.ChangePhaseAndSubPhase(phaseNumber,subPhaseNumber);
                return;
            }

            if (isSkipButton)
            {
                phaseChanger.DeactivateWarning();
                return;
            }

            Application.Quit();
        }
    }
}
