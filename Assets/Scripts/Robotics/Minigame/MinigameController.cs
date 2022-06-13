using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public int Score { get; set; }

    float _deadzone;
    float _radius;
    float _verticalRadius;

    [SerializeField,Min(0)]
    int _obstaclesPerRound;
    [SerializeField]
    float _objectRadius;

    [SerializeField]
    Transform _endEffector;
    [SerializeField,Min(float.Epsilon)]
    float _endEffectorRadius;

    [SerializeField]
    GameObject _goalPrefab;
    [SerializeField]
    GameObject _obstaclePrefab;

    GameObject[] _authoredChildren = System.Array.Empty<GameObject>();

    /// <summary>
    /// Generate a set of obstacles and a goal point to navigate towards.
    /// </summary>
    public void GenerateRound()
    {
        ClearSpawnedObjects();

        Vector3[] positions = new Vector3[_obstaclesPerRound + 1];

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 pos;
            bool isInvalid;
            do
            {
                isInvalid = false;

                pos = GenerateRandomPoint();
                Vector3 worldPos = pos + transform.position;

                // determine if the point is in range of the end effector
                if (IsInsideRange(worldPos, _objectRadius, _endEffector.position, _endEffectorRadius))
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
                if(i == 0)
                {
                    Instantiate(_goalPrefab, pos, Quaternion.identity, this.transform);
                }
                else
                {
                    Instantiate(_obstaclePrefab, pos, Quaternion.identity, this.transform);
                }
            } while (isInvalid);
        }
    }

    // Removes any active game objects spawned by the minigame.
    public void ClearSpawnedObjects()
    {
        if (_authoredChildren == null)
            return;

        for (int i = 0; i < _authoredChildren.Length; i++)
        {
            Destroy(_authoredChildren[i]);
        }
    }

    /// <summary>
    /// Generates a random point in a standing cylinder that can have a hole in the center.
    /// </summary>
    /// <returns> A random point in a standing cylinder.</returns>
    Vector3 GenerateRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitCircle;
        randomPoint *= _radius;

        var deadzoneOffset = randomPoint.normalized * _deadzone;

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
