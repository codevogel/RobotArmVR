using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControlDirectorTime))]
public class OnboardingProgress : MonoBehaviour
{
    Step _currentStep = Step.PlayerStarted;
    ControlDirectorTime _controlDirectorTime;

    private void Awake()
    {
        _controlDirectorTime = GetComponent<ControlDirectorTime>();
    }

    public void HandleOnPause()
    {
        switch (_currentStep)
        {
            case Step.PlayerStarted:
                //_currentStep = Step.WaitingForTeleport;
                //break;
            //case Step.PlayerTeleported:
                _currentStep = Step.WaitingForGrabbedCan;
                break;
            case Step.PlayerGrabbedCan:
                _currentStep = Step.WaitingForThrowingCanAway;
                break;
            case Step.PlayerThrewAwayCan:
                _currentStep = Step.WaitingForToggledRadio;
                break;
            case Step.PlayerTurnedToggledRadio:
                break;
            default:
                break;
        }
    }

    public enum Step
    {
        PlayerStarted,
        WaitingForTeleport,
        PlayerTeleported,
        WaitingForGrabbedCan,
        PlayerGrabbedCan,
        WaitingForThrowingCanAway,
        PlayerThrewAwayCan,
        WaitingForToggledRadio,
        PlayerTurnedToggledRadio
    }

    public void HandleTeleport()
    {
        if (_currentStep == Step.WaitingForTeleport)
        {
            _currentStep = Step.PlayerTeleported;
            _controlDirectorTime.Resume();
        }
    }

    public void HandleCanGrabbed()
    {
        if(_currentStep == Step.WaitingForGrabbedCan)
        {
            _currentStep = Step.PlayerGrabbedCan;
            _controlDirectorTime.Resume();
        }
    }

    public void HandleCanThrownAway()
    {
        if(_currentStep == Step.WaitingForThrowingCanAway)
        {
            _currentStep = Step.PlayerThrewAwayCan;
            _controlDirectorTime.Resume();
        }
    }

    public void HandleRadioTurnedOff()
    {
        if(_currentStep == Step.WaitingForToggledRadio)
        {
            _currentStep = Step.PlayerTurnedToggledRadio;
            _controlDirectorTime.Resume();
        }
    }
}
