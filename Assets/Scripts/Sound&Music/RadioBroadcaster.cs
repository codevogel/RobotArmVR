using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Broadcasts to multiple radios, only plays on the closest listener.
/// </summary>
public class RadioBroadcaster : MonoBehaviour
{

    public AudioClip clip;

    public RadioListener[] listeners;
    public Transform player;

    private RadioListener oldListener;

    private bool broadcasting;

    private void Start()
    {
        StartBroadcast();
    }

    /// <summary>
    /// Starts or stops broadcast based on current broadcast state
    /// </summary>
    public void StartStopBroadcast()
    {
        if (broadcasting)
        {
            StopBroadcast();
            return;
        }
        StartBroadcast();
    }

    /// <summary>
    /// Starts broadcasting
    /// </summary>
    private void StartBroadcast()
    {
        oldListener = GetClosestListener();
        oldListener.StartPlaying(clip);
        broadcasting = true;
    }

    /// <summary>
    /// Stop broadcasting
    /// </summary>
    public void StopBroadcast()
    {
        broadcasting = false;
        foreach (RadioListener listener in listeners)
        {
            listener.StopPlaying();
        }
    }

    private void Update()
    {
        if (broadcasting)
        {
            BroadcastToClosestListener();
        }
    }

    /// <summary>
    /// Play broadcast on closest radio
    /// </summary>
    private void BroadcastToClosestListener()
    {
        SwitchListener(GetClosestListener());
    }


    /// <summary>
    /// Gets the closest listener
    /// </summary>
    /// <returns>The closest RadioListener</returns>
    private RadioListener GetClosestListener()
    {
        RadioListener closest = null;

        float shortestDist = float.MaxValue;
        foreach (RadioListener listener in listeners)
        {
            float thisDist = Vector3.Distance(player.position, listener.transform.position);
            if (thisDist < shortestDist)
            {
                shortestDist = thisDist;
                closest = listener;
            }
        }

        return closest;
    }

    /// <summary>
    /// Switch the old closest listener for the current closest
    /// </summary>
    /// <param name="closest">the current closest listener</param>
    public void SwitchListener(RadioListener closest)
    {
        if (closest != oldListener)
        {
            oldListener.StopPlaying();
            closest.StartPlaying(clip);
            closest.SetTime(oldListener.GetTime());
            oldListener = closest;
        }
    }
}
