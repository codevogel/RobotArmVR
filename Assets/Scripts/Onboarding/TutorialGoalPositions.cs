using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TutorialGoalPositions : MonoBehaviour
{
    [SerializeField, Min(0)] float timeRequiredInside;
    [SerializeField] OnTriggerColliderFilter[] _colliders;
    [SerializeField] UnityEvent _onAdvancedStep;
    [SerializeField] UnityEvent _onCompletion;

    int _currentGoal;
    Coroutine _advanceCoroutine;

    /// <summary>
    /// Resets the active goal positions and starts with the first one.
    /// </summary>
    public void Begin()
    {
        Assert.IsTrue(_colliders.Length > 0, $"({this.gameObject.name} {nameof(TutorialGoalPositions)}) Must have at least one collider goal.");

#if UNITY_EDITOR
        if(_currentGoal < _colliders.Length)
#else
        Assert.IsTrue(_currentGoal < _colliders.Length, $"{nameof(_currentGoal)} is in an illegal state");
#endif
            _colliders[_currentGoal].gameObject.SetActive(false);

        _currentGoal = 0;
        _colliders[0].gameObject.SetActive(true);
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