using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

// Alias om verwarring tussen System.Random en UnityEngine te voorkomen
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a random clip on the audio source.
    /// </summary>
    public void PlayClip()
    {
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.name} .Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsTrue(_clips.Length > 0, $"No audio clips in {this.name}. ");
        
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
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.name} .Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsTrue(_clips.Length > 0, $"No audio clips in {this.name}. ");
        Assert.IsTrue(minInterval >= 0, "Minimum interval cannot be lower than 0 seconds.");
        Assert.IsTrue(minInterval <= maxInterval, "Minimum interval cannot be higher than the maximum interval.");


        if(playImmediately)
            _audioSource.PlayOneShot(_clips[UnityRandom.Range(0, _clips.Length)]);

        while (true)
        {
            yield return new WaitForSeconds(UnityRandom.Range(minInterval,maxInterval));

            _audioSource.PlayOneShot(_clips[UnityRandom.Range(0, _clips.Length)]);
        }
    }
}
