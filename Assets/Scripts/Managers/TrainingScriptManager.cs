using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrainingScriptManager : MonoBehaviour
{
    [SerializeField] private TypeWriterText textWriter;
    [SerializeField] private PlayableDirector timeLine;
    [SerializeField] private ControlDirectorTime timeLineController;

    private TextAsset trainingScriptJSON;
    private TrainingScript trainingScript;
    private Phase[] phases;
    private Phase currentPhase;
    private SubPhase currentSubPhase;

    public static TrainingScriptManager Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        trainingScriptJSON = Resources.Load("JSON/TrainingScript") as TextAsset;
        trainingScript = JsonUtility.FromJson<TrainingScript>(trainingScriptJSON.text);
        phases = trainingScript.phases;
    }

    public void ChangePhase(int phaseNumber)
    {
        currentPhase = phases[phaseNumber];

        CheckTimeLine(currentPhase.time);
    }

    public void ChangeSubPhase(int subPhaseNumber)
    {
        currentSubPhase = currentPhase.subPhases[subPhaseNumber];
        textWriter.Write(currentSubPhase.message);

        CheckTimeLine(currentSubPhase.time);
    }

    private void CheckTimeLine(int newTime)
    {
        int timeDifference = Math.Abs(Mathf.RoundToInt((float)timeLine.time * 60) - newTime);

        if (timeDifference > 5)
        {
            timeLine.time = newTime / 60f;
        }
    }

    public void PauseTimeLine()
    {
        timeLineController.Pause();
    }

    public void ResumeTimeLine()
    {
        timeLineController.Resume();
    }

    [Serializable]
    public class TrainingScript
    {
        public Phase[] phases;
    }

    [Serializable]
    public class Phase
    {
        public string phaseName;
        public SubPhase[] subPhases;
        public int time;
    }

    [Serializable]
    public class SubPhase
    {
        public int subPhaseNumber;
        public string message;
        public int time;
    }
}
