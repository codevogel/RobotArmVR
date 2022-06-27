using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Manipulates the time of the timeline
/// </summary>
public class ControlDirectorTime : MonoBehaviour
{
    private PlayableDirector _director;
    private PlayState _currentState;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }

    /// <summary>
    /// Sets the time of the timeline to a specified time
    /// </summary>
    /// <param name="time">The time it needs to change to</param>
    public void SetTime(float time)
    {
        _director.Pause();
        _director.time = time / 60f;
        _director.Resume();
    }
    
    /// <summary>
    /// Start the timeline
    /// </summary>
    public void Play()
    {
        _currentState = PlayState.Playing;
        _director.Play();
    }

    /// <summary>
    /// Pause the timeline
    /// </summary>
    public void Pause()
    {
        _currentState = PlayState.Paused;
        _director.Pause();
    }

    /// <summary>
    /// Pause the timeline if the pressence is lost
    /// </summary>
    public void ForcePause()
    {
        _director.Pause();
    }

    /// <summary>
    /// Resume the timeline
    /// </summary>
    public void Resume()
    {
        _currentState = PlayState.Playing;
        _director.Resume();
    }
    
    /// <summary>
    /// Resume the timeline if the pressence is regained
    /// </summary>
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
