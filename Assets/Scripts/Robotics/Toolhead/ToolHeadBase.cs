using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

[RequireComponent(typeof(Rigidbody),typeof(XRGrabInteractable))]
public abstract class ToolHeadBase : MonoBehaviour
{
    [SerializeField] private Vector3 _localHeadAttachOffset;
    [SerializeField] private Vector3 _rotation;
    private ToolHeadController _controller;

    public bool Attachable { get; set; }
    public Rigidbody Rigidbody { get; private set; }
    public XRGrabInteractable GrabInteractable { get; private set; }

    public Vector3 HeadAttachOffset => Quaternion.Euler(_rotation) * Vector3.Scale(_localHeadAttachOffset, this.transform.localScale);

    private void Awake()
    {
        Attachable = true;

        Rigidbody = GetComponent<Rigidbody>();
        GrabInteractable = GetComponent<XRGrabInteractable>();
    }

    /// <summary>
    /// Attaches the tool to a robot arm and removes it from the physics system.
    /// </summary>
    /// <param name="toolHeadController"></param>
    public void AttachTool(ToolHeadController toolHeadController)
    {
        Debug.Log(Attachable);
        if(!Attachable)
            return;

        Attachable = false;

        _controller = toolHeadController;
        toolHeadController.CurrentTool = this;

        GrabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)GrabInteractable);//Check if this works after code cleanup

        transform.localPosition = Vector3.zero;
        transform.SetParent(toolHeadController.ToolHeadAttachPoint.transform, false);
        transform.localPosition -= HeadAttachOffset;
        transform.localRotation = Quaternion.Euler(_rotation);

        Rigidbody.isKinematic = true;
    }

    /// <summary>
    /// Detaches the tool from a robot arm.
    /// </summary>
    public void DetachTool(SelectEnterEventArgs _)
    {
        if(_controller != null && _controller.CurrentTool == this)
        {
            transform.localScale = Vector3.one;
            _controller.CurrentTool = null;
            _controller = null;
        }
    }

    /// <summary>
    /// Makes the tool usable again by the physics system.
    /// </summary>
    public void ReleaseTool(SelectExitEventArgs _)
    {
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
    }

    /// <summary>
    /// Set the current tool state using raw values.
    /// </summary>
    /// <param name="toolStateValue"></param>
    public abstract void SetToolState(int toolStateValue);

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        var previousColor = Gizmos.color;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position + (Quaternion.Euler(_rotation - this.transform.localEulerAngles + this.transform.eulerAngles) * Vector3.Scale(_localHeadAttachOffset, this.transform.localScale)), 0.025f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.transform.position + (Quaternion.Euler(this.transform.eulerAngles) * Vector3.Scale(_localHeadAttachOffset, this.transform.localScale)), 0.0125f);

        Gizmos.color = previousColor;
    }
#endif
}

public abstract class ToolHeadBase<TToolState> : ToolHeadBase
    where TToolState : unmanaged, Enum
{
    public TToolState CurrentState { get; protected set; }

    /// <summary>
    /// Set the tool's current state.
    /// </summary>
    /// <param name="toolState"></param>
    public abstract void SetToolState(TToolState toolState);

    /// <summary>
    /// Set the tool's current state.
    /// </summary>
    /// <remarks> Prefer using <see cref="SetToolState(TToolState)"/> instead to avoid boxing.</remarks>
    /// <param name="toolStateValue"></param>
    public override void SetToolState(int toolStateValue)
    {
#if UNITY_EDITOR
        // safety check die alleen in editor wordt gedaan
        var values = Enum.GetValues(typeof(TToolState));
        bool found = false;
        foreach (var value in values)
        {
            if(toolStateValue == (int)value)
                found = true;
        }

        Assert.IsTrue(found, $"{toolStateValue} is not a valid state of the tool.");
#endif

        // slechte hack met boxen om dit te laten werken (RIP geen Unsafe.As<,>())
        SetToolState((TToolState)(object)toolStateValue);
    }
}
