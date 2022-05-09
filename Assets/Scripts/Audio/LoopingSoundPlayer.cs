using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class LoopingSoundPlayer : MonoBehaviour, ISoundPlayer
{
    private AudioSource _audioSource;

    [field: SerializeField] public int IntroEndTime { get; set; }
    [field: SerializeField] public int OutroStartTime { get; set; }

    private LoopState _loopState;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource.playOnAwake)
            _loopState = LoopState.Intro;
    }

    /// <inheritdoc/>
    public void PlayClip()
    {
        Assert.IsNotNull(_audioSource.outputAudioMixerGroup, $"No mixer group in {this.gameObject.name}. Audio source should have a mixer group assigned in order to get the most control out of it.");
        Assert.IsNotNull(_audioSource.clip, $"No audio clip in {this.gameObject.name}.");

        // reset the clip to the start
        _audioSource.timeSamples = 0;

        _loopState = LoopState.Intro;
        _audioSource.Play();
    }

    /// <summary>
    /// Begins the outro of the audio clip.
    /// </summary>
    public void StopClip()
    {
        if (_loopState == LoopState.NotPlaying)
            return;

        _loopState = LoopState.Outro;

        _audioSource.timeSamples = OutroStartTime;
    }

    private void Update()
    {
        if (_audioSource.isPlaying)
        {
            switch (_loopState)
            {
                case LoopState.Intro:
                    if (_audioSource.timeSamples > IntroEndTime)
                        _loopState = LoopState.Main;
                    break;
                case LoopState.Main:
                    if (_audioSource.timeSamples >= OutroStartTime)
                        _audioSource.timeSamples = IntroEndTime;
                    break;
                case LoopState.Outro:
                    if (_audioSource.timeSamples == _audioSource.clip.samples)
                        _loopState = LoopState.NotPlaying;
                    break;
                case LoopState.NotPlaying:
                    // doe niets
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }

    private enum LoopState
    {
        NotPlaying,
        Intro,
        Main,
        Outro
    }
}
