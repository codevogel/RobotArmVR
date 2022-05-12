using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnboardingTrashCan : MonoBehaviour
{
    [SerializeField] GameObject _trash;
    [SerializeField] UnityEvent _trashThrownAway;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _trash)
        {
            _trashThrownAway.Invoke();
        }
    }
}
