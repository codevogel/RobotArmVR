using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScriptManager : MonoBehaviour
{
    [SerializeField]private TypeWriterText textWriter;

    private TextAsset trainingScriptJSON;
    private TrainingScript trainingScript;
    private Phase[] phases;
    private Phase currentPhase;
    private SubPhase currentSubPhase;

    // Start is called before the first frame update
    void Start()
    {
        trainingScriptJSON = Resources.Load("JSON/TrainingScript") as TextAsset;
        trainingScript = JsonUtility.FromJson<TrainingScript>(trainingScriptJSON.text);
        phases = trainingScript.phases;
    }

    public void ChangePhase(int phaseNumber, int subPhaseNumber)
    {
        currentPhase = phases[phaseNumber];
        currentSubPhase = phases[phaseNumber].subPhases[subPhaseNumber];
        textWriter.Write(currentSubPhase.message);
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    [Serializable]
    public class SubPhase
    {
        public int subPhaseNumber;
        public string message;
    }
}
