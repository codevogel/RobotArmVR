using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Swaps positions and rotations for transforms from list A with those from list B
/// </summary>
public class Swapper : MonoBehaviour
{

    [Header("Objects to swap (in order!)")]
    [Tooltip("Lists object A for the A/B swap")]
    public List<Transform> toSwapA;
    [Tooltip("Lists object B for the A/B swap")]
    public List<Transform> toSwapB;

    /// <summary>
    /// Swaps positions and rotations of objects from list A with those from list B
    /// </summary>
    public void Swap()
    {
        for (int i = 0; i < toSwapA.Count; i++)
        {
            Transform a = toSwapA[i];
            Transform b = toSwapB[i];
            a.gameObject.SetActive(false);
            b.gameObject.SetActive(false);

            Vector3 tmpPos = a.position;
            Quaternion tmpRot = a.rotation;
            a.position = b.position;
            a.rotation = b.rotation;
            b.position = tmpPos;
            b.rotation = tmpRot;

            a.gameObject.SetActive(true);
            b.gameObject.SetActive(true);

            List<Transform> tmp = toSwapA;
            toSwapA = toSwapB;
            toSwapB = tmp;
        }
    }
}
