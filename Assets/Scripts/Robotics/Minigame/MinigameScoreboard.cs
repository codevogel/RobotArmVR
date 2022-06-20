#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the statistics of the minigame session on a canvas and keeps it up to date.
/// </summary>
[DisallowMultipleComponent]
public class MinigameScoreboard : MonoBehaviour
{
    /// <summary>
    /// The minigame that will be observed by this scoreboard.
    /// </summary>
    [SerializeField, Tooltip("The minigame that will be observed by this scoreboard.")]
    MinigameController _minigame;

    /// <summary>
    /// The text mesh that will be used to display the current remaining time.
    /// </summary>
    [SerializeField, Tooltip("The text mesh that will be used to display the current remaining time.")]
    TextMeshProUGUI _timeRemainingMesh;

    /// <summary>
    /// The text mesh that will be used to display the current score.
    /// </summary>
    [SerializeField, Tooltip("The text mesh that will be used to display the current score.")]
    TextMeshProUGUI _scoreMesh;

    /// <summary>
    /// The image that acts as a visual count down by redusing the circular fill.
    /// </summary>
    [SerializeField, Tooltip("The image that acts as a visual count down by redusing the circular fill.")]
    Image _radialIndicator;

    /// <summary>
    /// Sets up the scoreboard to listen to when the minigame gets finished.
    /// </summary>
    private void Start()
    {
#if UNITY_ASSERTIONS
        Assert.IsNotNull(_minigame, $"{this.name} does not have a minigame set to observe.");
        Assert.IsNotNull(_radialIndicator, $"{this.name} does not have an image set to manipulate the fill for.");
        Assert.IsNotNull(_timeRemainingMesh, $"{this.name} does not have a text mesh pro instance set to write the remaining time to.");
        Assert.IsNotNull(_scoreMesh, $"{this.name} does not have a text mesh pro instance set to write the score to.");
#endif

        // make sure the GUI is updated one last time when the minigame is finished
        _minigame.OnMinigameFinished.AddListener(UpdateGUI);
    }

    /// <summary>
    /// Refreshes the GUI only when the minigame is being played.
    /// </summary>
    void Update()
    {
        if (_minigame.IsBeingPlayed)
        {
            UpdateGUI();
        }
    }

    /// <summary>
    /// Updates the GUI with the current score and remaining time.
    /// </summary>
    public void UpdateGUI()
    {
        // set the radial fill using the remaining time
        _radialIndicator.fillAmount = _minigame.TimeRemaining / _minigame.TimeLimit;

        // display the current remaining time with 1 decimal unless it's over
        _timeRemainingMesh.text = _minigame.TimeRemaining == 0 ?
            _minigame.TimeRemaining.ToString() :
            _minigame.TimeRemaining.ToString("0.0");

        // display the current score
        _scoreMesh.text = _minigame.Score.ToString();
    }
}
