using UnityEngine;

/// <summary>
/// A behavior that forces the object to always look towards the player
/// </summary>
public class LookAtPlayer : MonoBehaviour
{
    /// <summary>
    /// Determines if the rotation should be flipped.
    /// </summary>
    /// <remarks>
    /// Used for things like UI Elements.
    /// </remarks>
    [field: SerializeField, Tooltip("Determines if the rotation should be flipped. Used for things like UI Elements.")]
    public bool Flipped { get; set; }

    /// <summary>
    /// The player that should be looked at.
    /// </summary>
    private Transform _player;

    /// <summary>
    /// Registers the camera transform.
    /// </summary>
    private void Start()
    {
        _player = Camera.main.transform;
    }

    /// <summary>
    /// Changes the object's transform to always look at the main camera.
    /// </summary>
    private void Update()
    {
        if (Flipped)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _player.position);
        }
        else
        {
            transform.LookAt(_player);
        }
    }
}
