using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionGripTool : ToolHeadBase
{
    [SerializeField] bool _succ;
    [SerializeField] Rigidbody _affectedObject;

    private void Update()
    {
        if (!_succ && _affectedObject != null)
        {
            _affectedObject.transform.SetParent(null, true);
            _affectedObject.useGravity = true;
            _affectedObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_succ && _affectedObject == null && other.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            _affectedObject = rigidbody;
            rigidbody.useGravity = false;
            _affectedObject.transform.SetParent(this.transform, true);
        }
    }

//#if UNITY_EDITOR
//    protected override void OnDrawGizmos()
//    {
//        base.OnDrawGizmos();
//    }
//#endif
}
