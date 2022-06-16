#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A mechanism to play a minigame where the player moves into positions while avoiding obstacles.
/// </summary>
public class MinigameController : MonoBehaviour
{
    /// <summary> 
    /// The objects spawned by the minigame. 
    /// </summary>
    readonly private List<OnTriggerColliderFilter> _authoredChildren = new List<OnTriggerColliderFilter>();

    /// <summary> 
    /// The coroutine that starts and ends the minigame. 
    /// </summary>
    private Coroutine _minigameCoroutine;

    /// <summary> 
    /// The time at which the minigame has been started. 
    /// </summary>
    private float _timeStarted;

    /// <summary>
    /// The current score of the minigame session.
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// The time remaining until the minigame is over.
    /// </summary>
    public float TimeRemaining => Mathf.Clamp(TimeLimit - (Time.time - _timeStarted), 0, float.MaxValue);

    /// <summary>
    /// Shows if the minigame is currently being played.
    /// </summary>
    public bool IsBeingPlayed => _minigameCoroutine != null;

    /// <summary>
    /// The amount of time in seconds the player has to score points in a minigame session.
    /// </summary>
    [field: SerializeField, Min(0), Tooltip("The amount of time in seconds the player has to score points in a minigame session.")]
    public float TimeLimit { get; set; } = 120;

    [SerializeField, Min(0), Tooltip("The amount of additional obstacles to try spawning in to increase difficulty.")]
    private int _obstaclesPerRound;

    [SerializeField, Tooltip("The pointer the player has to move into the goals to score points.")]
    private Collider _pointer;

    [Header("Bounds")]
    [SerializeField, Min(float.Epsilon), Tooltip("The radius in which goals and obstacles will be spawned.")]
    private float _radius;

    [SerializeField, Min(0), Tooltip("The radius from the center in which no objects can be spawned.")]
    private float _deadzone;

    [SerializeField, Min(0), Tooltip("The vertical extend in which goals and obstacles can be spawned.")]
    private float _verticalRadius;

    [SerializeField, Tooltip("The vertical offset to move the space objects can be spawned in up and down.")]
    private float _verticalOffset;

    [Header("Prefabs")]
    [SerializeField, Tooltip("The prefab used as a template to spawn in goal objects")]
    private OnTriggerColliderFilter _goalPrefab;

    [SerializeField, Tooltip("The prefab used as a template to spawn in obstacle objects")]
    private OnTriggerColliderFilter _obstaclePrefab;

    /// <summary>
    /// The event that fires whenever the minigame gets started.
    /// </summary>
    [field: Header("Events")]
    [field: SerializeField, Tooltip("The event that fires whenever the minigame gets started.")]
    public UnityEvent OnMinigameStarted { get; private set; }

    /// <summary>
    /// The event that fires whenever the time limit of the minigame has been met.
    /// </summary>
    [field: SerializeField, Tooltip("The event that fires whenever the time limit of the minigame has been met.")]
    public UnityEvent OnMinigameFinished { get; private set; }

    /// <summary>
    /// The event that fires whenever the pointer has touched a goal.
    /// </summary>
    [field: SerializeField, Tooltip("The event that fires whenever the pointer has touched a goal.")]
    public UnityEvent OnGoalTouched { get; private set; }

    /// <summary>
    /// The event that fires whenever the pointer has touched an obstacle.
    /// </summary>
    [field: SerializeField, Tooltip("The event that fires whenever the pointer has touched an obstacle.")]
    public UnityEvent OnObstacleTouched { get; private set; }

    /// <summary>
    /// Resets the minigame stats and starts the minigame.
    /// </summary>
    public void BeginMinigame()
    {
        Score = 0;
        _timeStarted = Time.time;

        if (_minigameCoroutine != null)
            StopCoroutine(_minigameCoroutine);

        _minigameCoroutine = StartCoroutine(MinigameCoroutine());
    }

    /// <summary>
    /// Tells the minigame to stop running and cleans up any left over objects.
    /// </summary>
    /// <remarks> This will not call <see cref="OnMinigameFinished"/></remarks>
    public void EndMinigame()
    {
        if (_minigameCoroutine != null)
        {
            StopCoroutine(_minigameCoroutine);
            _minigameCoroutine = null;
        }

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
#if UNITY_ASSERTIONS
        Assert.IsNotNull(_goalPrefab, $"{this.name} does not have a goal prefab set.");
        Assert.IsNotNull(_obstaclePrefab, $"{this.name} does not have an obstacle prefab set.");
        Assert.IsNotNull(_pointer, $"{this.name} does not have a pointer set and can't register when a goal or obstacle has been touched.");
        Assert.IsTrue(_radius > 0, $"{this.name} has an invalid radius. The spawn radius must be higher than 0.");
        Assert.IsTrue(_deadzone >= 0, $"{this.name} has an invalid deadzone. The deadzone radius must be at least 0.");
        Assert.IsTrue(_radius > _deadzone, $"{this.name} has an invalid deadzone. The deadzone must be smaller than the spawn radius.");
#endif

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
                isInvalid = false;

                if (iterations++ > 100)
                {
                    Debug.LogWarning("Exceeded max iteration count for a randomized position.");

                    // if the goal position has not been created keep on forcing it
                    if (i == 0)
                    {
                        continue;
                    }

                    break;
                }

                pos = GenerateRandomPoint();
                Vector3 worldPos = pos + transform.position;

                // prevent spawning inside of the flange
                if (IsOverlappingSphere(worldPos, radius, _pointer.transform.position, _pointer.bounds.extents.x * 1.2f))
                {
                    isInvalid = true;
                    continue;
                }

                // check previously generated points to prevent overlap
                int childrenCount = _authoredChildren.Count;
                for (int j = 0; j < childrenCount; j++)
                {
                    if (IsOverlappingSphere(worldPos, radius, _authoredChildren[j].transform.position, radius))
                    {
                        isInvalid = true;
                        break;
                    }
                }

                // if the state is invalid, stop early and iterate again
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

    /// <summary>
    /// Determines if two spheres are overlapping.
    /// </summary>
    /// <param name="posA"> The position of the first sphere.</param>
    /// <param name="radiusA"> The radius of the first sphere.</param>
    /// <param name="posB"> The position of the first sphere.</param>
    /// <param name="radiusB"> The radius of the first sphere.</param>
    /// <returns> True if two spheres overlap.</returns>
    private static bool IsOverlappingSphere(Vector3 posA, float radiusA, Vector3 posB, float radiusB)
    {
        Vector3 difference = posA - posB;
        return difference.sqrMagnitude < (radiusA * radiusA) + (radiusB * radiusB);
    }
}
