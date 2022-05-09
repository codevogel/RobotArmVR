using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionGripTool : ToolHeadBase<SuctionGripTool.ToolState>
{
    private Rigidbody _affectedObject;

    // todo: kijk of dit vervangen kan worden zo dat dit niet elke keer triggert.
    // moet kijken naar hoe je colliders kan checken zonder dit.
    private void OnTriggerStay(Collider other)
    {
        if (CurrentState == ToolState.Grab && _affectedObject == null && other.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            EnableSuctionGrab(rigidbody);
        }
    }

    public void EnableSuctionGrab(Rigidbody grabbedObject)
    {
        Debug.Log("Grabbing");
        if(_affectedObject == null)
        {
            _affectedObject = grabbedObject;
            grabbedObject.useGravity = false;
            grabbedObject.isKinematic = true;
            _affectedObject.transform.SetParent(this.transform, true);
        }
    }
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

    public enum ToolState
    {
        Release,
        Grab
    }
}
