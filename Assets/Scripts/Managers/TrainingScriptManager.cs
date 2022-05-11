using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using static UIHoverButton;

public class TrainingScriptManager : MonoBehaviour
{
    [SerializeField] private TypeWriterText textWriter;
    [SerializeField] private PlayableDirector timeLine;
    [SerializeField] private ControlDirectorTime timeLineController;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject confirmationCanvas;

    public List<PhaseTransform> phasesPosition;

    private TextAsset trainingScriptJSON;
    private TrainingScript trainingScript;
    private Phase[] phases;
    private bool changeByButton;

    [HideInInspector]
    public Phase currentPhase;

    [HideInInspector]
    public SubPhase currentSubPhase;

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

    public void EndPhaseConfirmation()
    {
        confirmationCanvas.SetActive(true);
        confirmationCanvas.transform.GetChild(0).gameObject.SetActive(true);
        Transform backButton = confirmationCanvas.transform.GetChild(1);
        backButton.GetComponent<UIHoverButton>().chosenAction = HoverActions.RESTARTCURRENTPHASE;
        backButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Restart";
    }

    public void EndSubPhaseConfirmation()
    {
        confirmationCanvas.SetActive(true);
        confirmationCanvas.transform.GetChild(0).gameObject.SetActive(false);
        Transform backButton = confirmationCanvas.transform.GetChild(1);
        backButton.GetComponent<UIHoverButton>().chosenAction = HoverActions.RESTARTCURRENTSUBPHASE;
        backButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Back";
    }

    public void CloseCanvas()
    {
        confirmationCanvas.SetActive(false);
        textWriter.Clear();
    }

    public void ChangePhase(int phaseNumber)
    {
        currentPhase = phases[phaseNumber];
        ResumeTimeLine();
        CheckTimeLineDifference(currentPhase.startTime);
    }

    public void ChangeSubPhase(int subPhaseNumber)
    {
        textWriter.Clear();
        currentSubPhase = currentPhase.subPhases[subPhaseNumber];

        if (!changeByButton)
        {
            textWriter.Write(currentSubPhase.message);
        }
        changeByButton = false;

        CheckTimeLineDifference(currentSubPhase.startTime);
    }

    public void ChangebyButton(int subPhaseNumber)
    {
        changeByButton = true;
        ChangeSubPhase(subPhaseNumber);
    }

    private void CheckTimeLineDifference(int newTime)
    {
        int timeDifference = Math.Abs(Mathf.RoundToInt((float)timeLine.time * 60) - newTime);
        if (timeDifference > 5)
        {
            timeLine.time = newTime / 60f;
            Vector3 newPosition= phasesPosition[currentPhase.phaseNumber].subPhaseTransforms[currentSubPhase.subPhaseNumber].subPhaseTransform.position;
            player.transform.position = new Vector3(newPosition.x,player.transform.position.y,newPosition.z);
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

    #region Json reading
    [Serializable]
    public class TrainingScript
    {
        public Phase[] phases;
    }

    [Serializable]
    public class Phase
    {
        public string phaseName;
        public int phaseNumber;
        public SubPhase[] subPhases;
        public int startTime;
    }

    [Serializable]
    public class SubPhase
    {
        public int subPhaseNumber;
        public string message;
        public int startTime;
    }
    #endregion

    #region Transform saving
    [Serializable]
    public class SubPhaseTransform
    {
        public Transform subPhaseTransform;
    }

    [Serializable]
    public class PhaseTransform
    {
        public Transform phaseTransform;
        public List<SubPhaseTransform> subPhaseTransforms;
    }
    #endregion
}