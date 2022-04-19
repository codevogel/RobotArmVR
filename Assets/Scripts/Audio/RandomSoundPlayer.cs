using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

// Alias om verwarring tussen System.Random en UnityEngine te voorkomen
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour, ISoundPlayer
{
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(_audioSource);
    }

    /// <summary>
    /// Plays a random clip on the audio source.
    /// </summary>
    public void PlayClip()
    {
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.gameObject.name}. Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsTrue(_clips.Length > 0, $"No audio clips in {this.name}. ");

#if UNITY_EDITOR
        if (_audioSource.clip != null)
            Debug.Log($"Audio clip set in the audio source of {this.gameObject.name} will be ignored and can be left empty.");
#endif
        
        _audioSource.PlayOneShot(_clips[UnityRandom.Range(0, _clips.Length)]);
    }

    /// <summary>
    /// Plays a random clip on interval on the audio source
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
        Assert.IsTrue(_clips.Length > 0, $"No audio clips in {this.name}.");
        Assert.IsTrue(minInterval >= 0, "Minimum interval cannot be lower than 0 seconds.");
        Assert.IsTrue(minInterval <= maxInterval, "Minimum interval cannot be higher than the maximum interval.");

#if UNITY_EDITOR
        if (_audioSource.clip != null)
            Debug.Log($"Audio clip set in the audio source of {this.name} will be ignored and can be left empty.");
#endif

        if (playImmediately)
            _audioSource.PlayOneShot(_clips[UnityRandom.Range(0, _clips.Length)]);

        while (true)
        {
            yield return new WaitForSeconds(UnityRandom.Range(minInterval,maxInterval));

            _audioSource.PlayOneShot(_clips[UnityRandom.Range(0, _clips.Length)]);
        }
    }
}