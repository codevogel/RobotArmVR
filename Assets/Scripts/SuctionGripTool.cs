using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionGripTool : ToolHeadBase
{
    [SerializeField] private bool _succ;
    [SerializeField] private Rigidbody _affectedObject;

    // todo: verwijder Update & OnTriggerStay als tool controls werken.
    private void Update()
    {
        if (!_succ)
        {
            DisableSuctionGrab();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_succ && _affectedObject == null && other.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            EnableSuctionGrab(rigidbody);
        }
    }

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

}
