using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class MinigameController : MonoBehaviour
{
    readonly private List<OnTriggerColliderFilter> _authoredChildren = new List<OnTriggerColliderFilter>();
    private Coroutine _minigameCoroutine;
    private float _timeStarted;

    public int Score { get; private set; }
    public float TimeRemaining => Mathf.Clamp(TimeLimit - (Time.time - _timeStarted), 0, float.MaxValue);
    public bool IsBeingPlayed => _minigameCoroutine != null;

    [field: SerializeField, Min(0)]
    public float TimeLimit { get; set; } = 120;
    [SerializeField]
    private Collider _pointer;

    [Header("Bounds")]
    [SerializeField, Min(float.Epsilon)]
    private float _radius;
    [SerializeField, Min(0)]
    private float _deadzone;
    [SerializeField, Min(0)]
    private float _verticalRadius;
    [SerializeField]
    private float _verticalOffset;
    [SerializeField, Min(float.Epsilon)]
    private float _pointerRadius;

    [Header("Obstacles")]
    [SerializeField, Min(0)]
    private int _obstaclesPerRound;
    //[SerializeField]
    //private float _objectRadius;

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

    /// <summary>
    /// Kickstarts the minigame and stops after the time limit is reached.
    /// </summary>
    private IEnumerator MinigameCoroutine()
    {
        GenerateRound();
        yield return new WaitForSeconds(TimeLimit);
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

        int toSpawn = _obstaclesPerRound + 1;
        float radius = _goalPrefab.GetComponent<Collider>().bounds.extents.x * 1.2f;

        for (int i = 0; i < toSpawn; i++)
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

               

                if (Physics.CheckSphere(worldPos, radius, -1, QueryTriggerInteraction.Collide))
                {
                    isInvalid = true;
                    continue;
                }

                int childrenCount = _authoredChildren.Count;
                // check previously generated points to prevent overlap
                for (int j = 0; j < childrenCount; j++)
                {
                    if (IsInsideRange(worldPos, radius, _authoredChildren[j].transform.position, radius))
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

                    _authoredChildren.Add(goal);
                }
                else
                {
                    var obstacle = Instantiate(_obstaclePrefab, this.transform);
                    obstacle.transform.localPosition = pos;
                    obstacle.Filter = _pointer;

                    obstacle.OnTriggerEnterRelay.AddListener(() =>
                    {
                        Score--;
                        OnObstacleTouched?.Invoke();

                        GenerateRound();
                    });

                    _authoredChildren.Add(obstacle);
                }
            } while (isInvalid);
        }
    }

    /// <summary>
    /// Removes any active game objects spawned by the minigame.
    /// </summary>
    public void ClearSpawnedObjects()
    {
        for (int i = 0; i < _authoredChildren.Count; i++)
        {
            if (_authoredChildren[i] != null)
                Destroy(_authoredChildren[i].gameObject);
        }

        _authoredChildren.Clear();
    }

    /// <summary>
    /// Generates a random point in a standing cylinder that can have a hole in the center.
    /// </summary>
    /// <returns> A random point in a standing cylinder.</returns>
    private Vector3 GenerateRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitCircle;
        var deadzoneOffset = randomPoint.normalized * _deadzone;

        randomPoint *= Random.Range(_deadzone, _radius - _deadzone);

        randomPoint += deadzoneOffset;

        randomPoint.z = randomPoint.y;
        randomPoint.y = (Random.value * _verticalRadius) + _verticalOffset;

        return randomPoint;
    }

    private static bool IsInsideRange(Vector3 posA, float radiusA, Vector3 posB, float radiusB)
    {
        Vector3 difference = posA - posB;
        return difference.sqrMagnitude < (radiusA * radiusA) + (radiusB * radiusB);
    }
}
