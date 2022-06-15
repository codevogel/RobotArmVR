using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class MinigameController : MonoBehaviour
{
    readonly private List<GameObject> _authoredChildren = new List<GameObject>();
    private Coroutine _minigameCoroutine;
    private float _timeStarted;

    public int Score { get; private set; }
    public float TimeRemaining => Mathf.Clamp(_timeLimit - (Time.time - _timeStarted), 0, float.MaxValue);
    public bool IsBeingPlayed => _minigameCoroutine != null;

    [SerializeField, Min(0)]
    private float _timeLimit;
    [SerializeField]
    private Collider _pointer;
    
    [Header("Bounds")]
    [SerializeField, Min(float.Epsilon)]
    private float _radius;
    [SerializeField, Min(0)]
    private float _deadzone;
    [SerializeField, Min(0)]
    private float _verticalRadius;
    [SerializeField, Min(float.Epsilon)]
    private float _pointerRadius;

    [Header("Obstacles")]
    [SerializeField, Min(0)]
    private int _obstaclesPerRound;
    [SerializeField]
    private float _objectRadius;

    [Header("Goal")]
    [SerializeField]
    private OnTriggerColliderFilter _goalPrefab;
    [SerializeField]
    private OnTriggerColliderFilter _obstaclePrefab;

    [field: Header("Events")]
    [field: SerializeField]
    public UnityEvent OnMinigameStarted { get; private set; }
    [field: SerializeField]
    public UnityEvent OnMinigameFinished { get; private set; }

    [field: SerializeField]
    public UnityEvent OnGoalTouched { get; private set; }

    [field: SerializeField]
    public UnityEvent OnObstacleTouched { get; private set; }

    /// <summary>
    /// Resets the minigame stats and starts the minigame.
    /// </summary>
    public void BeginMinigame()
    {
        Score = 0;
        _timeStarted = Time.time;
        _minigameCoroutine = StartCoroutine(MinigameCoroutine());
    }

    /// <summary>
    /// Tells the minigame to stop running and cleans up any left over objects.
    /// </summary>
    /// <remarks> This will not call <see cref="OnMinigameFinished"/></remarks>
    public void EndMinigame()
    {
        if (_minigameCoroutine != null)
            StopCoroutine(_minigameCoroutine);

        ClearSpawnedObjects();
    }

    private IEnumerator MinigameCoroutine()
    {
        GenerateRound();
        yield return new WaitForSeconds(_timeLimit);
        ClearSpawnedObjects();
        _minigameCoroutine = null;
    }

    /// <summary>
    /// Generate a set of obstacles and a goal point to navigate towards.
    /// </summary>
    public void GenerateRound()
    {
        Assert.IsNotNull(_goalPrefab);
        Assert.IsNotNull(_obstaclePrefab);
        Assert.IsNotNull(_pointer);
        Assert.IsTrue(_radius > 0);
        Assert.IsTrue(_radius > _deadzone);

        ClearSpawnedObjects();

        Vector3[] positions = new Vector3[_obstaclesPerRound + 1];

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 pos;
            bool isInvalid;
            int iterations = default;
            do
            {
                if (iterations++ > 100)
                {
                    Debug.LogError("Oh shit, er gaat iets goed mis of ik heb heel veel pech.");
                    break;
                }

                isInvalid = false;

                pos = GenerateRandomPoint();
                Vector3 worldPos = pos + transform.position;

                // determine if the point is in range of the end effector
                if (IsInsideRange(worldPos, _objectRadius, _pointer.transform.position, _pointerRadius))
                {
                    isInvalid = true;
                    continue;
                }

                // check previously generated points to prevent overlap
                for (int j = 0; j < i; j++)
                {
                    if (IsInsideRange(worldPos, _objectRadius, positions[j] + transform.position, _objectRadius))
                    {
                        isInvalid = true;
                        break;
                    }
                }
                if (isInvalid)
                    continue;


                // spawn a goal for the first index, obstacles for anything after
                if (i == 0)
                {
                    var goal = Instantiate(_goalPrefab, this.transform);
                    goal.transform.localPosition = pos;
                    goal.Filter = _pointer;
                    goal.OnTriggerEnterRelay.AddListener(() =>
                    {
                        Score++;
                        OnGoalTouched?.Invoke();

                        GenerateRound();
                    });

                    _authoredChildren.Add(goal.gameObject);
                }
                else
                {
                    var obstacle = Instantiate(_obstaclePrefab, this.transform);
                    obstacle.transform.localPosition = pos;
                    obstacle.Filter = _pointer;

                    obstacle.OnTriggerExitRelay.AddListener(() =>
                    {
                        Score--;
                        OnObstacleTouched?.Invoke();

                        GenerateRound();
                    });

                    _authoredChildren.Add(obstacle.gameObject);
                }
            } while (isInvalid);
        }
    }

    // Removes any active game objects spawned by the minigame.
    public void ClearSpawnedObjects()
    {
        for (int i = 0; i < _authoredChildren.Count; i++)
        {
            if (_authoredChildren[i] != null)
                Destroy(_authoredChildren[i]);
        }

        _authoredChildren.Clear();
    }

    /// <summary>
    /// Generates a random point in a standing cylinder that can have a hole in the center.
    /// </summary>
    /// <returns> A random point in a standing cylinder.</returns>
    Vector3 GenerateRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitCircle;
        var deadzoneOffset = randomPoint.normalized * _deadzone;

        randomPoint *= Random.Range(_deadzone, _radius - _deadzone);

        randomPoint += deadzoneOffset;

        randomPoint.z = randomPoint.y;
        randomPoint.y = Random.value * _verticalRadius;

        return randomPoint;
    }

    private static bool IsInsideRange(Vector3 posA, float radiusA, Vector3 posB, float radiusB)
    {
        Vector3 difference = posA - posB;
        return difference.sqrMagnitude < (radiusA * radiusA) + (radiusB * radiusB);
    }
}
