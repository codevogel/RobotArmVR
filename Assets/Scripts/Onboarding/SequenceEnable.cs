using UnityEngine;

/// <summary>
/// A component for handling enabling game objects in sequence.
/// </summary>
public class SequenceEnable : MonoBehaviour
{
    /// <summary>
    /// The game objects to be enabled in order of the collection.
    /// </summary>
    [SerializeField, Tooltip("The game objects to be enabled in order of the collection.")] 
    private GameObject[] _gameObjects;

    /// <summary>
    /// The index of the current object to be enabled.
    /// </summary>
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

    /// <summary>
    /// Turns off the current object and enables the next object in the sequence.
    /// </summary>
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

    /// <summary>
    /// Resets the sequence of enabled game objects.
    /// </summary>
    public void BeginSequence()
    {
        if (_gameObjects.Length > _index)
            _gameObjects[_index].SetActive(false);

        _index = 0;

        _gameObjects[_index].SetActive(true);
    }


    /// <summary>
    /// Disables the current game object in the sequence.
    /// </summary>
    public void StopSequence()
    {
        if (_gameObjects.Length > _index)
            _gameObjects[_index].SetActive(false);
    }
}
