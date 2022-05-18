using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHoverButton : MonoBehaviour
{
    [SerializeField] private float hoverActivationTime;

    [HideInInspector] public HoverActions chosenAction;

    private Image hoverIndicator;
    private float hoverTime;
    private int newSubPhase;
    private bool timerActive;

    private void Start()
    {
        hoverIndicator = transform.GetChild(1).GetComponent<Image>();
        hoverIndicator.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (hoverTime>=hoverActivationTime)
        {
            hoverIndicator.gameObject.SetActive(false);
            ActivationAction();
        }

        if (timerActive)
        {
            hoverIndicator.gameObject.SetActive(true);
            hoverTime += Time.deltaTime;
            hoverIndicator.fillAmount=hoverTime/hoverActivationTime;
        }
    }

    public void SubPhaseToRevertTo(int subPhaseAmount)
    {
        newSubPhase = subPhaseAmount;
    }

    private void ActivationAction()
    {
        if (transform.name.Equals("Resume"))
        {
            Debug.Log("ContinueActivate");
            chosenAction = HoverActions.CONTINUE;
            Debug.Log("CONTUINUEDONE");
        }
        else if (transform.name.Equals("Back"))
        {
            TextMeshProUGUI backButton = GetComponentInChildren<TextMeshProUGUI>();
            if (backButton.text.Equals("Restart"))
            {
                chosenAction = HoverActions.RESTARTCURRENTPHASE;
            }
            else if (backButton.text.Equals("Back"))
            {
                chosenAction = HoverActions.RESTARTCURRENTSUBPHASE;
            }
        }

        switch (chosenAction)
        {
            case HoverActions.RESTARTCURRENTPHASE:
                TrainingScriptManager.Instance.ChangePhase(TrainingScriptManager.Instance.currentPhase.phaseNumber);
                break;
            case HoverActions.RESTARTCURRENTSUBPHASE:
                TrainingScriptManager.Instance.ChangebyButton(newSubPhase);
                break;
            case HoverActions.CONTINUE:
                Debug.Log("NewTimeBefore");
                TrainingScriptManager.Instance.Newtime();
                Debug.Log("NewTimeAfter");
                break;
        }
        hoverTime = 0;
        TrainingScriptManager.Instance.CloseCanvas();
    }

    public void TimerActive(bool input)
    {
        timerActive = input;
        if (!timerActive)
        {
            hoverTime = 0;
            hoverIndicator.gameObject.SetActive(false);
        }
    }

    public enum HoverActions
    {
        RESTARTCURRENTPHASE,
        RESTARTCURRENTSUBPHASE,
        CONTINUE
    }
}
