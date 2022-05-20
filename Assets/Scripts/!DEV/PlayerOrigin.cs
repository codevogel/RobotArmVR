using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrigin : MonoBehaviour
{
    public Transform player;

    private void Awake()
    {
        player.position = transform.position;
        player.rotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1,2,1));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
