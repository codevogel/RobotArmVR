using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TrainingScriptManager : MonoBehaviour
{
    [SerializeField] private TypeWriterText textWriter;
    [SerializeField] private PlayableDirector timeLine;
    [SerializeField] private ControlDirectorTime timeLineController;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject confirmationCanvas;
    [SerializeField] private GameObject fadeVisionCanvas;
    [SerializeField] private PhaseChanger phaseChanger;
    [SerializeField] private float differenceAllowance;

    public List<Transform> phasesPosition;

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

    /// <summary>
    /// Activates the confirmation canvas with the restart message displayed
    /// </summary>
    public void EndPhaseConfirmation()
    {
        confirmationCanvas.SetActive(true);
        Transform backButton = confirmationCanvas.transform.GetChild(0);
        backButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Restart";
        confirmationCanvas.transform.GetChild(0).gameObject.SetActive(true);
        confirmationCanvas.transform.GetChild(1).gameObject.SetActive(true);
        confirmationCanvas.transform.GetChild(2).gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the confirmation canvas with the back message displayed
    /// </summary>
    public void EndSubPhaseConfirmation()
    {
        confirmationCanvas.SetActive(true);
        Transform backButton = confirmationCanvas.transform.GetChild(0);
        backButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Back";
        confirmationCanvas.transform.GetChild(0).gameObject.SetActive(true);
        confirmationCanvas.transform.GetChild(1).gameObject.SetActive(true);
        confirmationCanvas.transform.GetChild(2).gameObject.SetActive(false);
    }

    /// <summary>
    /// Deactivates the confirmation canvas
    /// </summary>
    public void ContinueButton()
    {
        confirmationCanvas.SetActive(true);
        confirmationCanvas.transform.GetChild(0).gameObject.SetActive(false);
        confirmationCanvas.transform.GetChild(1).gameObject.SetActive(false);
        confirmationCanvas.transform.GetChild(2).gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates a change in subphase when a trigger is activated
    /// </summary>
    /// <param name="trigger">The specific trigger for a subphase</param>
    public void ActivateTrigger(int trigger)
    {
        switch (trigger)
        {
            //Teleport case
            case 0:
                if (currentSubPhase.subPhaseNumber== 5)
                {
                    Newtime();
                }
                break;
            //Point case
            case 1:
                if (currentSubPhase.subPhaseNumber == 7)
                { 
                    Newtime();
                }
                break;
            //Radio case
            case 2:
                if (currentSubPhase.subPhaseNumber == 9)
                {
                    Newtime();
                }
                break;
            //Snap turn
            case 3:
                if (currentSubPhase.subPhaseNumber == 10)
                {
                    Newtime();
                }
                break;
            //Safety Button
            case 4:
                if (currentSubPhase.subPhaseNumber == 3)
                {
                    Newtime();
                }
                break;
            //Switch Axis
            case 5:
                if (currentSubPhase.subPhaseNumber == 12)
                {
                    Newtime();
                }
                break;
            //Switch Mode
            case 6:
                if (currentSubPhase.subPhaseNumber == 0)
                {
                    Newtime();
                }
                break;
            //Left / Right
            case 7: if(currentSubPhase.subPhaseNumber == 2)
                {
                    Newtime();
                }
                break;
            //Back / Forth
            case 8:
                if (currentSubPhase.subPhaseNumber == 3)
                {
                    Newtime();
                }
                break;
            //Up / Down
            case 9:
                if (currentSubPhase.subPhaseNumber == 4)
                {
                    Newtime();
                }
                break;
        }
    }

    /// <summary>
    /// Closes the confirmation canvas
    /// </summary>
    public void CloseCanvas()
    {
        confirmationCanvas.SetActive(false);
        textWriter.Clear();
    }

    /// <summary>
    /// Change between phases 
    /// </summary>
    /// <param name="phaseNumber">What phase it needs to change to</param>
    public void ChangePhase(int phaseNumber)
    {
        currentPhase = phases[phaseNumber];
        currentSubPhase = phases[phaseNumber].subPhases[0];
        CheckTimeLineDifference(currentPhase.startTime);

        CheckPhaseButton();
    }

    /// <summary>
    /// Change the current subphase within a phase
    /// </summary>
    /// <param name="subPhaseNumber">Subphase it needs to change to</param>
    public void ChangeSubPhase(int subPhaseNumber)
    {
        textWriter.Clear();
        currentSubPhase = currentPhase.subPhases[subPhaseNumber];

        if (!changeByButton)
        {
            textWriter.Write(currentSubPhase.message);
        }
        changeByButton = false;

        //Originally used in the confirmation menu
        //CheckTimeLineDifference(currentSubPhase.startTime);

        CheckPhaseButton();
    }

    /// <summary>
    /// Change the subphase through a button
    /// </summary>
    /// <param name="subPhaseNumber">Subphase it needs to change to</param>
    public void ChangebyButton(int subPhaseNumber)
    {
        changeByButton = true;
        ChangeSubPhase(subPhaseNumber);

        CheckPhaseButton();
    }

    /// <summary>
    /// Activates a button on the phasechange canvas
    /// </summary>
    private void CheckPhaseButton()
    {
        phaseChanger.ActivatePhaseButton(currentPhase.phaseNumber);
    }

    /// <summary>
    /// Checks difference between timeline time and new (sub)phase time
    /// </summary>
    /// <param name="newTime">Time to change to</param>
    private void CheckTimeLineDifference(int newTime)
    {
        if (currentSubPhase.subPhaseNumber==15)
        {
            Debug.Log(newTime + "newtime, " + timeLine.time*60 + "timeline time");
        }
        int timeDifference = Math.Abs(Mathf.RoundToInt((float)timeLine.time * 60) - newTime);
        if (timeDifference > differenceAllowance)
        {
            StartCoroutine(Teleport(newTime));
        }
    }

    /// <summary>
    /// Teleports the player with a fade transition
    /// </summary>
    /// <param name="newTime">Time to change to</param>
    /// <returns></returns>
    private IEnumerator Teleport(int newTime)
    {
        fadeVisionCanvas.SetActive(true);
        fadeVisionCanvas.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeVision");

        //Wait on the animation of the vision fader
        yield return new WaitForSeconds(1f);
        Vector3 newPosition = phasesPosition[currentPhase.phaseNumber].position;
        player.transform.position = new Vector3(newPosition.x, player.transform.position.y, newPosition.z);
        timeLine.Pause();
        timeLine.time = newTime / 60f;
        timeLine.Resume();

        //Wait on the animation of the vision coming back
        yield return new WaitForSeconds(1f);

        fadeVisionCanvas.SetActive(false);
    }

    /// <summary>
    /// Pauses the timeline
    /// </summary>
    public void PauseTimeLine()
    {
        timeLineController.Pause();
    }

    /// <summary>
    /// Resumes the timeline
    /// </summary>
    public void ResumeTimeLine()
    {
        timeLineController.Resume();
    }

    /// <summary>
    /// Set the timeline to the endtime of the current subphase
    /// </summary>
    public void Newtime()
    {
        timeLineController.SetTime(currentSubPhase.endTime);
    }

    #region Json reading
    /// <summary>
    /// Holds all the phases in the JSON file
    /// </summary>
    [Serializable]
    public class TrainingScript
    {
        public Phase[] phases;
    }

    /// <summary>
    /// A specific phase in the JSON file holds: name, number, all subphases for that phase, start and end time
    /// </summary>
    [Serializable]
    public class Phase
    {
        public string phaseName;
        public int phaseNumber;
        public SubPhase[] subPhases;
        public int startTime;
        public float endTime;
    }

    /// <summary>
    /// A specific subphase within a phase holds: a number, message, start and end time
    /// </summary>
    [Serializable]
    public class SubPhase
    {
        public int subPhaseNumber;
        public string message;
        public int startTime;
        public float endTime;
    }
    #endregion
}