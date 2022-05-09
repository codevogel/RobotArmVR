using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnteredTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent _onPlayerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _onPlayerEntered.Invoke();
        }
    }
}
