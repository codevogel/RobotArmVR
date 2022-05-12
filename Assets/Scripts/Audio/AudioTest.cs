using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
public class AudioTest : MonoBehaviour
{
    ISoundPlayer _player;

    private void Awake()
    {
        Assert.IsTrue(this.TryGetComponent<ISoundPlayer>(out _player));

        Debug.LogWarning($"Don't forget to remove the Audio Test component on {gameObject.name} once you are done.");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("start"))
            _player.PlayClip();
        if(GUILayout.Button("stop"))
            _player.StopClip();
    }
}