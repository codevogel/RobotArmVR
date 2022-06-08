using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private int phaseNumber;
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
                Debug.Log(transform.gameObject.name);
                TrainingScriptManager.Instance.ChangePhase(phaseNumber);
                return;
            }

            if (isSkipButton)
            {
                phaseChanger.DeactivateWarning();
                phaseChanger.UnlockAllButtons();
                return;
            }

            Application.Quit();
        }
    }
}
