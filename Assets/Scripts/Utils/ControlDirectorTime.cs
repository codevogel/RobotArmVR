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
    public void Pause() => _director.Pause();
    public void Resume() => _director.Resume();
}
