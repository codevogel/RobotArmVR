using UnityEngine;

/// <summary>
/// A toolhead for grabbing rigidbodies with the robot arm.
/// </summary>
public class SuctionGripTool : ToolHeadBase<SuctionGripTool.ToolState>
{
    /// <summary>
    /// The object currently picked up.
    /// </summary>
    private Rigidbody _affectedObject;

    /// <summary>
    /// Attempts to pick up a rigidbody if no other has been picked up.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // Make sure the tool head is in the right state and the target has a rigidbody
        if (CurrentState == ToolState.Grab && _affectedObject == null && other.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            EnableSuctionGrab(rigidbody);
        }
    }

    /// <summary>
    /// Causes the tool head to start attempting to grab a rigid body.
    /// </summary>
    /// <param name="grabbedObject"></param>
    public void EnableSuctionGrab(Rigidbody grabbedObject)
    {
        if(_affectedObject == null)
        {
            _affectedObject = grabbedObject;
            grabbedObject.useGravity = false;
            grabbedObject.isKinematic = true;
            _affectedObject.transform.SetParent(this.transform, true);
        }
    }

    /// <summary>
    /// Releases the grabbed rigidbody and drops it.
    /// </summary>
    public void DisableSuctionGrab()
    {
        if(_affectedObject != null)
        {
            _affectedObject.transform.SetParent(null, true);
            _affectedObject.useGravity = true;
            _affectedObject.isKinematic = false;
            _affectedObject = null;
        }
    }

    /// <inheritdoc/>
    public override void SetToolState(ToolState toolState)
    {
        switch (toolState)
        {
            case ToolState.Release:
                DisableSuctionGrab();
                break;
            case ToolState.Grab:
                break;
        }

        CurrentState = toolState;
    }

    /// <summary>
    /// The states in which the suction tool head can be.
    /// </summary>
    public enum ToolState
    {
        /// <summary>
        /// The toolhead should not be picking up items.
        /// </summary>
        Release,

        /// <summary>
        /// The toolhead should attempt to hold onto a rigidbody.
        /// </summary>
        Grab
    }
}
