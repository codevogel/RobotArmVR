using UnityEngine;

public class SequenceEnable : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjects;
    private int _index;

    /// <summary>
    /// Starts listening to phase change events.
    /// </summary>
    private void OnEnable()
    {
        TouchButton.OnPhaseChange += HandlePhaseChanged;
    }

    /// <summary>
    /// Stops listening to phase change events.
    /// </summary>
    private void OnDisable()
    {
        TouchButton.OnPhaseChange -= HandlePhaseChanged;
    }

    /// <summary>
    /// Stops the tutorial when the phase is changed.
    /// </summary>
    private void HandlePhaseChanged(int _) => StopSequence();

    public void Next()
    {
        if (_gameObjects.Length <= _index)
            return;
        
        _gameObjects[_index].SetActive(false);

        _index++;

        if (_gameObjects.Length <= _index)
            return;

        _gameObjects[_index].SetActive(true);
    }

    public void BeginSequence()
    {
        if (_gameObjects.Length > _index)
            _gameObjects[_index].SetActive(false);

        _index = 0;

        _gameObjects[_index].SetActive(true);
    }

    public void StopSequence()
    {
        if (_gameObjects.Length > _index)
            _gameObjects[_index].SetActive(false);
    }
}
