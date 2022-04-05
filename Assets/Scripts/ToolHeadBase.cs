using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ToolHeadBase : MonoBehaviour
{
    [SerializeField] private Vector3 _localHeadAttachOffset;
    [SerializeField] private Vector3 _rotation;

    public Rigidbody Rigidbody { get; private set; }

    public Vector3 HeadAttachOffset => Quaternion.Euler(_rotation) * Vector3.Scale(_localHeadAttachOffset, this.transform.localScale);

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void AttachTool(ToolHeadController toolHeadController)
    {
        transform.localPosition = Vector3.zero;
        transform.SetParent(toolHeadController.ToolHeadAttachPoint.transform, false);
        toolHeadController.CurrentTool = this;
        transform.localPosition -= HeadAttachOffset;
        transform.localRotation = Quaternion.Euler(_rotation);
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        var previousColor = Gizmos.color;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position + (Quaternion.Euler(_rotation - this.transform.localEulerAngles + this.transform.eulerAngles) * Vector3.Scale(_localHeadAttachOffset, this.transform.localScale)), 0.025f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(this.transform.position, Quaternion.Euler(_rotation + this.transform.eulerAngles) * (this.transform.up * 0.2f));

        Gizmos.color = previousColor;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Attach To Arm"))
        {
            var toolHeadController = GameObject.FindObjectOfType<ToolHeadController>();

            AttachTool(toolHeadController);
        }
    }
#endif
}
