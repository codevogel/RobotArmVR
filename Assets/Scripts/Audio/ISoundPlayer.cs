using System.Collections;

// Alias om verwarring tussen System.Random en UnityEngine te voorkomen
public interface ISoundPlayer
{
    /// <summary>
    /// Plays a clip on the audio source.
    /// </summary>
    void PlayClip();

    /// <summary>
    /// Plays a clip on interval on the audio source
    /// </summary>
    /// <remarks>
    /// Use with coroutines.
    /// </remarks>
    /// <param name="minInterval"> The minimum time in seconds before the sound can be played again.</param>
    /// <param name="maxInterval"> The maximum time in seconds before the sound can be played again.</param>
    /// <param name="playImmediately"> Determines if a clip should be played before a start interval period.</param>
    IEnumerator PlayClipOnInterval(float minInterval, float maxInterval, bool playImmediately = false);
}
