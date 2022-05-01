using UnityEngine;
using UnityEngine.Playables;

public class ControlDirectorTime : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Editor Only")]
    [SerializeField] private float _seconds;
    [SerializeField] private bool _setOnStart;
#endif
    private PlayableDirector _director;
    private PlayState _currentState;

    [SerializeField] GameObject Trash;
    [SerializeField] GameObject Trigger;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();

#if UNITY_EDITOR
        if (_setOnStart)
            SetTime(_seconds);
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// This version is only usable in the editor, use <seealso cref="SetTime(float)"/> instead.
    /// </summary>
    public void SetTime() => _director.time = _seconds;
#endif

    public void SetTime(float time) => _director.time = time;

    public void SetTimeTeleport(float time)
    {
        if (Trash.activeSelf == true)
        {
            _director.time = time;
        }
    }

    public void SetTimeGrab(float time)
    {
        if (Trigger.activeSelf == true)
        {
            _director.time = time;
        }
    }

    public void Play()
    {
        _currentState = PlayState.Playing;
        _director.Play();
    }

    public void Pause()
    {
        _currentState = PlayState.Paused;
        _director.Pause();
    }

    public void ForcePause()
    {
        _director.Pause();
    }

    public void Resume()
    {
        _currentState = PlayState.Playing;
        _director.Resume();
    }

    public void ForceResume()
    {
        switch (_currentState)
        {
            case PlayState.Playing:
                _director.Resume();
                break;
            case PlayState.Stopped:    
            case PlayState.Paused:
                break;

        }
    }

    private enum PlayState
    {
        Playing,
        Paused,
        Stopped
    }
}
