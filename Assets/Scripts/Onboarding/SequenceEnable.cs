using UnityEngine;

public class SequenceEnable : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjects;
    private int _index;

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
}
