using UnityEngine;
using UnityEngine.Playables;

public class SetDirectorTime : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private float _seconds;
    [SerializeField] private bool _setOnStart;
    private PlayableDirector _director;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();

        if (_setOnStart)
            SetTime();
    }

    public void SetTime() => _director.time = _seconds;
#endif
}
