using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <summary>
/// An authoring component for handling the tutorial for moving the end effector to a specific position.
/// </summary>
public class TutorialGoalPositions : MonoBehaviour
{
    /// <summary>
    /// The time you need to be inside of the goal to advance to the next step.
    /// </summary>
    [SerializeField, Min(0),Tooltip("The time you need to be inside of the goal to advance to the next step.")]
    private float timeRequiredInside;

    /// <summary>
    /// The goals that the user will have to move into.
    /// </summary>
    [SerializeField, Tooltip("The goals that the user will have to move into.")] 
    private OnTriggerColliderFilter[] _colliders;

    /// <summary>
    /// The event that fires when the sequence has begun.
    /// </summary>
    [SerializeField, Tooltip("The event that fires when the sequence has begun.")] 
    private UnityEvent _onBeginSequence;

    /// <summary>
    /// The event that fires when the user advances to the next step.
    /// </summary>
    [SerializeField, Tooltip("The event that fires when the user advances to the next step.")] 
    private UnityEvent _onAdvancedStep;

    /// <summary>
    /// The event that fires when the user has completed the training segment.
    /// </summary>
    [SerializeField, Tooltip("The event that fires when the user has completed the training segment.")] 
    private UnityEvent _onCompletion;

    /// <summary>
    /// The current step in the tutorial.
    /// </summary>
    int _currentGoal;

    /// <summary>
    /// The coroutine that waits for a certain amount of time before advancing to the next step.
    /// </summary>
    Coroutine _advanceCoroutine;

    /// <summary>
    /// Resets the active goal positions and starts with the first one.
    /// </summary>
    public void Begin()
    {
        Assert.IsTrue(_colliders.Length > 0, $"({this.gameObject.name} {nameof(TutorialGoalPositions)}) Must have at least one collider goal.");

        if (_currentGoal < _colliders.Length)
            _colliders[_currentGoal].gameObject.SetActive(false);

        _currentGoal = 0;
        _colliders[0].gameObject.SetActive(true);

        _onBeginSequence.Invoke();
    }

    /// <summary>
    /// Starts a coroutine to advance to the next goal after a set period of time.
    /// </summary>
    public void QueueAdvance() =>
        _advanceCoroutine = StartCoroutine(Advance());

    /// <summary>
    /// Cancels the coroutine to advance to the next goal after a set period of time.
    /// </summary>
    public void CancelAdvance()
    {
        if (_advanceCoroutine != null)
            StopCoroutine(_advanceCoroutine);
    }

    /// <summary>
    /// Set the current goal to the next one in line after a set amount of time.
    /// </summary>
    public IEnumerator Advance()
    {
        if (timeRequiredInside > 0)
            yield return new WaitForSeconds(timeRequiredInside);

        _colliders[_currentGoal].gameObject.SetActive(false);

        _currentGoal++;

        // check if this was the last goal
        if (_currentGoal == _colliders.Length)
        {
            _onCompletion.Invoke();
            _currentGoal = default;
        }
        else
        {
            _colliders[_currentGoal].gameObject.SetActive(true);
            _onAdvancedStep.Invoke();
        }
    }

    /// <summary>
    /// Set the current goal to the next one in line.
    /// </summary>
    public void AdvanceImmediately()
    {
        _colliders[_currentGoal].gameObject.SetActive(false);

        _currentGoal++;

        // check if this was the last goal
        if (_currentGoal == _colliders.Length)
        {
            _onCompletion.Invoke();
            _currentGoal = default;
        }
        else
        {
            _colliders[_currentGoal].gameObject.SetActive(true);
            _onAdvancedStep.Invoke();
        }
    }
}