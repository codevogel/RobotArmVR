using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayer
{
    /// <summary>
    /// Plays a clip on the audio source.
    /// </summary>
    void PlayClip();

    /// <summary>
    /// Stops a clip on the audio source.
    /// </summary>
    void StopClip();
}
