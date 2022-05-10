using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverButton : MonoBehaviour
{
    [SerializeField] private float hoverActivationTime;
    private float hoverTime;
    private bool timerActive;

    private void FixedUpdate()
    {
        if (hoverTime>=hoverActivationTime)
        {
            ActivationAction();
        }

        if (timerActive)
        {
            hoverTime += Time.deltaTime;
        }
    }

    private void ActivationAction()
    {
        Debug.Log("RightActivation");
    }

    public void TimerActive(bool input)
    {
        timerActive = input;
        if (!timerActive)
        {
            hoverTime = 0;
        }
    }
}
