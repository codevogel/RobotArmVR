using UnityEngine;

/// <summary>
/// Makes sure the assistant will look towards the player.
/// </summary>
public class AssistantLookTowardsPlayer : MonoBehaviour
{
    /// <summary>
    /// The angle offset to make the robot actually face towards the player.
    /// </summary>
    private const int _assistantOffset = 90;

    /// <summary>
    /// Determines if the assistant should be looking at the player at this moment.
    /// </summary>
    private bool _active = true;

    /// <summary>
    /// The target the robot will look at.
    /// </summary>
    /// <remarks> Will be the main camera by default.</remarks>
    [SerializeField, Tooltip("The target the robot will look at. Will be the main camera by default.")]
    Transform _target;

    /// <summary>
    /// The range of vertical rotation the assistant's head will make.
    /// </summary>
    [SerializeField, Tooltip("The range of vertical rotation the assistant's head will make.")]
    MinMaxFloat _headVerticalRotationBounds;

    /// <summary>
    /// Registers the main camera as target if no override is given in <see cref="_target"/>.
    /// </summary>
    private void Awake()
    {
        if (_target == null)
            _target = Camera.main.transform;
    }

    /// <summary>
    /// Sets the rotation of the assistant to look at the target when this behavior is active.
    /// </summary>
    void LateUpdate()
    {
        if (_active)
        {
            // make the head look at the target
            transform.LookAt(_target);

            // clamp the vertical swivel
            var oldRotation = transform.localRotation.eulerAngles;
            oldRotation.x = Mathf.Clamp(oldRotation.x > 180 ? (oldRotation.x - 360) : oldRotation.x, _headVerticalRotationBounds.Min, _headVerticalRotationBounds.Max);

            // swap x & z due to weird model rotations
            transform.localRotation = Quaternion.Euler(oldRotation.z, oldRotation.y + _assistantOffset, oldRotation.x);
        }
    }

    /// <summary>
    /// Sets the state for looking at the player.
    /// </summary>
    /// <param name="activate"> True if the assistant should be looking at the player. False to stop looking at the player.</param>
    public void Active(bool activate)
    {
        _active = activate;
    }
}
