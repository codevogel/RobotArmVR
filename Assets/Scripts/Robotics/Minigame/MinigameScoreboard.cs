#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

using TMPro;
using UnityEngine;
using UnityEngine.UI;


[DisallowMultipleComponent]
public class MinigameScoreboard : MonoBehaviour
{
    [SerializeField]
    MinigameController _minigame;
    [SerializeField]
    TextMeshProUGUI _timeRemainingMesh;
    [SerializeField]
    TextMeshProUGUI _scoreMesh;

    [SerializeField]
    Image _radialIndicator;

    private void Start()
    {
#if UNITY_ASSERTIONS
        Assert.IsNotNull(_minigame);
        Assert.IsNotNull(_radialIndicator);
        Assert.IsNotNull(_timeRemainingMesh);
        Assert.IsNotNull(_scoreMesh);
#endif

        // make sure the GUI is updated one last time when the minigame is finished
        _minigame.OnMinigameFinished.AddListener(UpdateGUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (_minigame.IsBeingPlayed)
        {
            UpdateGUI();
        }
    }

    public void UpdateGUI()
    {
        // set the radial fill using the remaining time
        _radialIndicator.fillAmount = _minigame.TimeRemaining / _minigame.TimeLimit;

        _timeRemainingMesh.text = _minigame.TimeRemaining == 0 ? 
            _minigame.TimeRemaining.ToString() : 
            _minigame.TimeRemaining.ToString("0.0");

        _scoreMesh.text = _minigame.Score.ToString();
    }
}
