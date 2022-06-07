using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    private int assistantOffset = 90;
    private bool active = true;

    [SerializeField]
    Transform _target;

    [SerializeField]
    MinMaxSliderFloat _headVerticalRotationBounds;

    private void Awake()
    {
        if (_target == null)
            _target = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (active)
        {
            // make the head look at the target
            transform.LookAt(_target);

            // clamp the vertical swivel
            var oldRotation = transform.localRotation.eulerAngles;
            Debug.Log("Before clamp: " + oldRotation.x);
            oldRotation.x = Mathf.Clamp(oldRotation.x > 180 ? (oldRotation.x - 360) : oldRotation.x, _headVerticalRotationBounds.Min, _headVerticalRotationBounds.Max);

            // swap x & z due to weird model rotations
            transform.localRotation = Quaternion.Euler(oldRotation.z, oldRotation.y + assistantOffset, oldRotation.x);
        }
    }

    public void Active(bool activate)
    {
        active = activate;
    }

}
