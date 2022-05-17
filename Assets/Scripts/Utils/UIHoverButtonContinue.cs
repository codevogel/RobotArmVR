using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHoverButtonContinue : MonoBehaviour
{
    [HideInInspector] public HoverActions chosenAction;

    private Image hoverIndicator;
    private float hoverTime;
    private int newSubPhase;
    private bool timerActive;

    private void Awake()
    {
        hoverIndicator = transform.GetChild(1).GetComponent<Image>();
        hoverIndicator.gameObject.SetActive(false);
        chosenAction = HoverActions.CONTINUE;
    }

    private void FixedUpdate()
    {
        if (hoverTime >= 1)
        {
            hoverIndicator.gameObject.SetActive(false);
            ActivationAction();
        }

        if (timerActive)
        {
            hoverIndicator.gameObject.SetActive(true);
            hoverTime += Time.deltaTime;
            hoverIndicator.fillAmount = hoverTime / 1;
        }
    }

    public void SubPhasesToRevert(int subPhaseAmount)
    {
        newSubPhase = subPhaseAmount;
    }

    private void ActivationAction()
    {
        switch (chosenAction)
        {
            case HoverActions.CONTINUE:
                TrainingScriptManager.Instance.Newtime();
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
        CONTINUE
    }
}
