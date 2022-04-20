using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

// Alias om verwarring tussen System.Random en UnityEngine te voorkomen
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, ISoundPlayer
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <inheritdoc/>
    public void PlayClip()
    {
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.gameObject.name}. Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsNotNull(_audioSource.clip, $"No audio clip in {this.gameObject.name}.");

        _audioSource.Play();
    }

    /// <summary>
    /// Plays a clip on interval on the audio source
    /// </summary>
    /// <remarks>
    /// Use with coroutines.
    /// </remarks>
    /// <param name="minInterval"> The minimum time in seconds before the sound can be played again.</param>
    /// <param name="maxInterval"> The maximum time in seconds before the sound can be played again.</param>
    /// <param name="playImmediately"> Determines if a random clip should be played before a start interval period.</param>
    public IEnumerator PlayClipOnInterval(float minInterval, float maxInterval, bool playImmediately = false)
    {
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.gameObject.name} .Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsNotNull(_audioSource.clip, $"No audio clips in {this.gameObject.name}.");
        Assert.IsTrue(minInterval >= 0, "Minimum interval cannot be lower than 0 seconds.");
        Assert.IsTrue(minInterval <= maxInterval, "Minimum interval cannot be higher than the maximum interval.");


        if (playImmediately)
            _audioSource.Play();

        while (true)
        {
            yield return new WaitForSeconds(UnityRandom.Range(minInterval, maxInterval));

            _audioSource.Play();
        }
    }

    /// <inheritdoc/>
    public void StopClip()
    {
        _audioSource.Stop();
    }
}
