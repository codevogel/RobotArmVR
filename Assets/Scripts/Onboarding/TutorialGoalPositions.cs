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

    public void Begin()
    {
        Assert.IsTrue(_colliders.Length > 0, $"({this.gameObject.name} {nameof(TutorialGoalPositions)}) Must have at least one collider goal.");

        _currentGoal = 0;
        _colliders[0].gameObject.SetActive(true);
    }

    public void QueueAdvance() =>
        _advanceCoroutine = StartCoroutine(Advance());

    public void CancelAdvance()
    {
        if (_advanceCoroutine != null)
            StopCoroutine(_advanceCoroutine);
    }

    public IEnumerator Advance()
    {
        if (timeRequiredInside > 0)
            yield return new WaitForSeconds(timeRequiredInside);

        _colliders[_currentGoal].gameObject.SetActive(false);

        _currentGoal++;

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