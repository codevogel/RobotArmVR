using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartStopBroadcast()
    {
        if (broadcasting)
        {
            StopBroadcast();
            return;
        }
        StartBroadcast();
    }

    private void StartBroadcast()
    {
        oldListener = GetClosestListener();
        oldListener.StartPlaying(clip);
        broadcasting = true;
    }

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

    private void BroadcastToClosestListener()
    {
        RadioListener closest = GetClosestListener();
        SwitchListener(closest);
    }

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
