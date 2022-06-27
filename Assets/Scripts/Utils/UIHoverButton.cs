using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI button which can be hovered over with a raycast
/// </summary>
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

    /// <summary>
    /// Saves the subphase to which the button will change
    /// </summary>
    /// <param name="subPhaseAmount">The number of the subphase</param>
    public void SubPhaseToRevertTo(int subPhaseAmount)
    {
        newSubPhase = subPhaseAmount;
    }

    /// <summary>
    /// Activates an action dependent on what function the button has
    /// </summary>
    private void ActivationAction()
    {
        if (transform.name.Equals("Resume") || transform.name.Equals("Continue"))
        {
            chosenAction = HoverActions.CONTINUE;
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
                TrainingScriptManager.Instance.Newtime();
                break;
        }
        hoverTime = 0;
        TrainingScriptManager.Instance.CloseCanvas();
    }

    /// <summary>
    /// Timer which the loadcircle uses
    /// </summary>
    /// <param name="input">If the timer should be active</param>
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
