using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TutorialGoalRotation : MonoBehaviour
{
    [SerializeField] Step[] _steps;
    [SerializeField, Range(0, 180)] float _tollerance;
    [SerializeField, Min(0)] float _timeRequired;

    [SerializeField] UnityEvent _onAdvancedStep;
    [SerializeField] UnityEvent _onCompletion;

    int _currentStepIndex;
    Coroutine _currentCoroutine;

    /// <summary>
    /// Resets the active goal rotation and starts with the first one.
    /// </summary>
    public void Begin()
    {
        Assert.IsTrue(_steps.Length > 0, $"({this.gameObject.name} {nameof(TutorialGoalPositions)}) Must have at least one rotation goal.");

        this.enabled = true;

        _currentStepIndex = default;

        // set the first highlight to the goal rotation
        var currentStep = _steps[_currentStepIndex];
        currentStep.Highlight.gameObject.SetActive(true);
        currentStep.Highlight.rotation = currentStep.Axis switch
        {
            Axis.X => Quaternion.Euler(currentStep.TargetRotation, 0, 0),
            Axis.Y => Quaternion.Euler(0, currentStep.TargetRotation, 0),
            Axis.Z => Quaternion.Euler(0, 0, currentStep.TargetRotation),
            _ => throw new System.NotImplementedException()
        };
    }

    private void FixedUpdate()
    {
        var currentStep = _steps[_currentStepIndex];

        Quaternion targetRotation = currentStep.Axis switch
        {
            Axis.X => Quaternion.Euler(currentStep.TargetRotation, 0, 0),
            Axis.Y => Quaternion.Euler(0, currentStep.TargetRotation, 0),
            Axis.Z => Quaternion.Euler(0, 0, currentStep.TargetRotation),
            _ => throw new System.NotImplementedException()
        };

        // counter rotate the highlight to make it appear as if it is not rotating
        currentStep.Highlight.localRotation = Quaternion.Inverse(currentStep.Observing.localRotation) * targetRotation;

        var angle = Quaternion.Angle(currentStep.Observing.localRotation, targetRotation);

        // if the observed axis rotation is close enough to the step's goal rotation, start advancing to the next step.
        if (angle <= _tollerance)
        {
            if (_currentCoroutine == null)
            {
                _currentCoroutine = StartCoroutine(Advance());
            }
        }
        else
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Set the current goal to the next one in line after a set amount of time.
    /// </summary>
    private IEnumerator Advance()
    {
        if (_timeRequired > 0)
            yield return new WaitForSeconds(_timeRequired);

        _currentCoroutine = null;

        // reset the highlight of the completed step
        var currentStep = _steps[_currentStepIndex];
        currentStep.Highlight.gameObject.SetActive(false);
        currentStep.Highlight.rotation = Quaternion.identity;

        _currentStepIndex++;

        // check if this was the last goal
        if (_currentStepIndex == _steps.Length)
        {
            _onCompletion.Invoke();
            _currentStepIndex = default;

            this.enabled = false;
        }
        else
        {
            currentStep = _steps[_currentStepIndex];
            currentStep.Highlight.gameObject.SetActive(true);

            _onAdvancedStep.Invoke();
        }
    }

    /// <summary>
    /// Set the current goal to the next one in line.
    /// </summary>
    public void AdvanceImmediately()
    {
        // reset the highlight of the completed step
        var currentStep = _steps[_currentStepIndex];
        currentStep.Highlight.gameObject.SetActive(false);
        currentStep.Highlight.rotation = Quaternion.identity;

        _currentStepIndex++;

        // check if this was the last goal
        if (_currentStepIndex == _steps.Length)
        {
            _onCompletion.Invoke();
            _currentStepIndex = default;
        }
        else
        {
            currentStep = _steps[_currentStepIndex];
            currentStep.Highlight.gameObject.SetActive(true);

            _onAdvancedStep.Invoke();

            this.enabled = false;
        }
    }

    [System.Serializable]
    public class Step
    {
        [field: SerializeField, Tooltip("The object who's rotation will be observed.")]
        public Transform Observing { get; private set; }

        [field: SerializeField, Tooltip("The object acting as a guide for the end position.")]
        public Transform Highlight { get; private set; }

        [field: SerializeField, Tooltip("The desired rotation of the axis")]
        public float TargetRotation { get; private set; }

        [field: SerializeField, Tooltip("The axis to be tested.")]
        public Axis Axis { get; private set; }
    }
    public enum Axis { X, Y, Z }
}


