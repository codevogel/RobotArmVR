using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrigin : MonoBehaviour
{
    public Transform player;

    /// <summary>
    /// Set player origin to this transform
    /// </summary>
    private void Awake()
    {
        player.position = transform.position;
        player.rotation = transform.rotation;
    }

    /// <summary>
    /// Draw player gizmo
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1,2,1));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
