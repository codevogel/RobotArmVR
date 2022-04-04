using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ToolHeadBase : MonoBehaviour
{
    [SerializeField] private Vector3 _localHeadAttachOffset;

    public Vector3 HeadAttachOffset => Vector3.Scale(_localHeadAttachOffset, this.transform.localScale);

    private void AttachTool(ToolHeadController toolHeadController)
    {
        transform.localPosition = Vector3.zero;
        transform.SetParent(toolHeadController.ToolHeadAttachPoint.transform, false);
        toolHeadController.CurrentTool = this;
        transform.localPosition -= HeadAttachOffset;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        var previousColor = Gizmos.color;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position + Vector3.Scale(_localHeadAttachOffset, this.transform.localScale), 0.03f);

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
