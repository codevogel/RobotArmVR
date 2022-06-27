using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <summary>
/// An authoring component for handling a per axis rotation tutorial.
/// </summary>
public class TutorialGoalRotation : MonoBehaviour
{
    /// <summary>
    /// The steps in the tutorial.
    /// </summary>
    [SerializeField, Tooltip("The steps in the tutorial.")]
    private Step[] _steps;

    /// <summary>
    /// The angle from the center of cone in which the rotation will be accepted for the steps.
    /// </summary>
    [SerializeField, Range(0, 180), Tooltip("The angle from the center of cone in which the rotation will be accepted for the steps.")]
    private float _tollerance;

    /// <summary>
    /// The time you need to be inside of the tollerance zone to advance to the next step.
    /// </summary>
    [SerializeField, Min(0), Tooltip("The time you need to be inside of the tollerance zone to advance to the next step.")]
    private float _timeRequired;

    /// <summary>
    /// Determines if the current rotation should be captured for advancing to the next step.
    /// </summary>
    [SerializeField, Tooltip("Determines if the current rotation should be captured for advancing to the next step.")]
    private bool _capture;

    /// <summary>
    /// The event that will fire when the user advances to the next step in the tutorial.
    /// </summary>
    [SerializeField, Tooltip("The event that will fire when the user advances to the next step in the tutorial.")]
    private UnityEvent _onAdvancedStep;

    /// <summary>
    /// The event that will fire when the user has gone through all steps in the tutorial.
    /// </summary>
    [SerializeField, Tooltip("The event that will fire when the user has gone through all steps in the tutorial.")]
    private UnityEvent _onCompletion;

    /// <summary>
    /// The parts of the robot arm that should be observed at the start of each step.
    /// </summary>
    [SerializeField, Tooltip("The parts of the robot arm that should be observed at the start of each step.")]
    private ArticulationBody[] _trackPerStep;

    /// <summary>
    /// Used to reset articulation drives.
    /// </summary>
    [SerializeField, Tooltip("Used to reset articulation drives.")]
    private LinearMovement _linearMovement;

    /// <summary>
    /// The initial state of the drives of the articulation bodies.
    /// </summary>
    private ArticulationDrive[] _initPosition = new ArticulationDrive[6];

    /// <summary>
    /// The current step in the tutorial.
    /// </summary>
    int _currentStepIndex;

    /// <summary>
    /// The coroutine that waits for a certain amount of time before advancing to the next step.
    /// </summary>
    Coroutine _advanceCoroutine;

    /// <summary>
    /// Determines if the tutorial is currently active.
    /// </summary>
    private bool _tutorialActive;

    /// <summary>
    /// Records the initial state of the articulation bodies.
    /// </summary>
    private void Start()
    {
        for (int x = 0; x < _initPosition.Length; x++)
        {
            _initPosition[x] = _trackPerStep[x].xDrive;
        }
    }

    /// <summary>
    /// Starts listening to phase change events.
    /// </summary>
    private void OnEnable()
    {
        TouchButton.OnPhaseChange += HandlePhaseChanged;
    }

    /// <summary>
    /// Stops listening to phase change events.
    /// </summary>
    private void OnDisable()
    {
        TouchButton.OnPhaseChange -= HandlePhaseChanged;
    }

    /// <summary>
    /// Stops the tutorial when the phase is changed.
    /// </summary>
    private void HandlePhaseChanged(int _) => Stop();

    /// <summary>
    /// Resets the active goal rotation and starts with the first one.
    /// </summary>
    public void Begin()
    {
        Assert.IsTrue(_steps.Length > 0, $"({this.gameObject.name} {nameof(TutorialGoalPositions)}) Must have at least one rotation goal.");

        this.enabled = true;

        Step currentStep;

        // turn off previously active highlight
        if(_currentStepIndex < _steps.Length)
        {
            currentStep = _steps[_currentStepIndex];
            currentStep.Highlight.gameObject.SetActive(false);
        }
        
        _currentStepIndex = default;

        // set the first highlight to the goal rotation
        currentStep = _steps[_currentStepIndex];
        currentStep.Highlight.gameObject.SetActive(true);
        currentStep.Highlight.rotation = currentStep.Axis switch
        {
            Axis.X => Quaternion.Euler(currentStep.TargetRotation, 0, 0),
            Axis.Y => Quaternion.Euler(0, currentStep.TargetRotation, 0),
            Axis.Z => Quaternion.Euler(0, 0, currentStep.TargetRotation),
            _ => throw new System.NotImplementedException()
        };

        RecordRotation();

        _tutorialActive = true;
    }

    /// <summary>
    /// Turn off the current highlight.
    /// </summary>
    public void Stop()
    {
        Step currentStep;

        // turn off active highlight
        if (_currentStepIndex < _steps.Length)
        {
            currentStep = _steps[_currentStepIndex];
            currentStep.Highlight.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles the rotation of the hologram and determines of the axis is in the right rotation.
    /// </summary>
    private void Update()
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

        // don't check for the angle if it should not be captured at this point in time
        if (!_capture)
            return;

        var angle = Quaternion.Angle(currentStep.Observing.localRotation, targetRotation);

        if (currentStep.AllowSymetrical)
        {
            angle = Mathf.Min(angle, 180 - angle);
        }

        // if the observed axis rotation is close enough to the step's goal rotation, start advancing to the next step.
        if (angle <= _tollerance)
        {
            if (_advanceCoroutine == null)
            {
                _advanceCoroutine = StartCoroutine(Advance());
            }
        }
        else
        {
            if (_advanceCoroutine != null)
            {
                StopCoroutine(_advanceCoroutine);
                _advanceCoroutine = null;
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

        _advanceCoroutine = null;

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

            RecordRotation();

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

            RecordRotation();

            _onAdvancedStep.Invoke();

            this.enabled = false;
        }
    }

    /// <summary>
    /// Records the rotational state of the observed axis per step.
    /// </summary>
    private void RecordRotation()
    {
        var currentStep = _steps[_currentStepIndex];

        currentStep.InitialRotations = new ArticulationDrive[_trackPerStep.Length];

        for (int i = 0; i < _trackPerStep.Length; i++)
        {
            currentStep.InitialRotations[i] = _trackPerStep[i].xDrive;
        }
    }

    /// <summary>
    /// Reset the rotation of the observed articulation bodies.
    /// </summary>
    public void ResetRotation()
    {
        if (_tutorialActive)
        {
            var currentStep = _steps[_currentStepIndex];

            Assert.IsNotNull(currentStep.Highlight, "Cannot call ResetRotation at this point, as no rotations have been recorded.");

            for (int i = 0; i < currentStep.InitialRotations.Length; i++)
            {
                _trackPerStep[i].xDrive = currentStep.InitialRotations[i];
                StartCoroutine(WaitForDrive());
            }
        }
        else
        {
            for (int x = 0; x < _initPosition.Length; x++)
            {
                _trackPerStep[x].xDrive = _initPosition[x];
                StartCoroutine(WaitForDrive());
            }
        }
    }

    /// <summary>
    /// Sets the current capture state for determining if the angle is within tollerance.
    /// </summary>
    /// <param name="value"> True if the current angle should be captured.</param>
    public void EnableCapture(bool value)
    {
        _capture = value;

        if (_advanceCoroutine != null)
        {
            StopCoroutine(_advanceCoroutine);
            _advanceCoroutine = null;
        }
    }

    /// <summary>
    /// waits for the drives to have cooled down.
    /// </summary>
    private IEnumerator WaitForDrive()
    {
        yield return new WaitForSeconds(0.2f);
        _linearMovement.followTarget[_linearMovement.currentRobot].position = _trackPerStep[5].transform.position;
    }

    /// <summary>
    /// A step in the rotation tutorial that can be configured to trach a specific axis.
    /// </summary>
    [System.Serializable]
    public class Step
    {
        /// <summary>
        /// The object who's rotation will be observed.
        /// </summary>
        [field: SerializeField, Tooltip("The object who's rotation will be observed.")]
        public Transform Observing { get; private set; }

        /// <summary>
        /// The object acting as a guide for the end position.
        /// </summary>
        [field: SerializeField, Tooltip("The object acting as a guide for the end position.")]
        public Transform Highlight { get; private set; }

        /// <summary>
        /// The desired rotation of the axis.
        /// </summary>
        [field: SerializeField, Tooltip("The desired rotation of the axis.")]
        public float TargetRotation { get; private set; }

        /// <summary>
        /// Determines if a mirror rotation should be accepted for this step.
        /// </summary>
        [field: SerializeField, Tooltip("Determines if a mirror rotation should be accepted for this step.")]
        public bool AllowSymetrical { get; private set; }

        /// <summary>
        /// The axis to be tested.
        /// </summary>
        [field: SerializeField, Tooltip("The axis to be tested.")]
        public Axis Axis { get; private set; }

        /// <summary>
        /// The initial rotation state of the observed bodies.
        /// </summary>
        [HideInInspector]
        public ArticulationDrive[] InitialRotations { get; set; }
    }

    /// <summary>
    /// The axis to track the rotation on.
    /// </summary>
    public enum Axis { X, Y, Z }
}


