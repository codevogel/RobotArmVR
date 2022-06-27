using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static FlexpendantUIManager;

#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

/// <summary>
/// The base class for <see cref="ToolHeadBase{TToolState}"/>
/// </summary>
/// <remarks> Prefer inheriting from <see cref="ToolHeadBase{TToolState}"/>.</remarks>
[RequireComponent(typeof(Rigidbody),typeof(XRGrabInteractable))]
public abstract class ToolHeadBase : MonoBehaviour
{
    /// <summary>
    /// The local position the tool head should have when attaching to the end effector.
    /// </summary>
    [SerializeField, Tooltip("The local position the tool head should have when attaching to the end effector.")] 
    private Vector3 _localHeadAttachOffset;

    /// <summary>
    /// The local rotation the toolhead should have when attaching to the end effector.
    /// </summary>
    [SerializeField, Tooltip("The local rotation the tool head should have when attaching to the end effector.")] 
    private Vector3 _rotation;

    /// <summary>
    /// The authoring component on the robot arm.
    /// </summary>
    private ToolHeadController _controller;

    /// <summary>
    /// Identifies what tool this is.
    /// </summary>
    [SerializeField, Tooltip("Identifies what tool this is.")]
    private int _toolId;

    /// <summary>
    /// Determines if the tool head is attachable to an end effector.
    /// </summary>
    public bool Attachable { get; set; }

    /// <summary>
    /// The tool head's rigidbody.
    /// </summary>
    public Rigidbody Rigidbody { get; private set; }

    /// <summary>
    /// The component used for handling grabbing objects.
    /// </summary>
    public XRGrabInteractable GrabInteractable { get; private set; }

    /// <summary>
    /// The offset the toolhead should be placed at.
    /// </summary>
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
        if(!Attachable)
            return;

        Attachable = false;

        _controller = toolHeadController;
        toolHeadController.CurrentTool = this;

        GrabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)GrabInteractable);

        // puts the toolhead at the right position and rotation
        transform.localPosition = Vector3.zero;
        transform.SetParent(toolHeadController.ToolHeadAttachPoint.transform, false);
        transform.localPosition -= HeadAttachOffset;
        transform.localRotation = Quaternion.Euler(_rotation);

        Rigidbody.isKinematic = true;

        FlexpendantUIManager.Instance.ChangeProperty(Properties.TOOL, _toolId);
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
            FlexpendantUIManager.Instance.ChangeProperty(Properties.TOOL, 0);
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
}

/// <summary>
/// The base class for a tool head component that can be attached to the end effector of a robot arm.
/// </summary>
/// <typeparam name="TToolState"> The set of states the tool head can be in.</typeparam>
public abstract class ToolHeadBase<TToolState> : ToolHeadBase
    where TToolState : unmanaged, Enum
{
    public TToolState CurrentState { get; protected set; }

    /// <summary>
    /// Sets the current state of the tool head.
    /// </summary>
    /// <param name="toolState"> The desired state of the tool head.</param>
    public abstract void SetToolState(TToolState toolState);

    /// <summary>
    /// Set the tool's current state.
    /// </summary>
    /// <remarks> Prefer using <see cref="SetToolState(TToolState)"/> instead to avoid boxing.</remarks>
    /// <param name="toolStateValue"></param>
    public override void SetToolState(int toolStateValue)
    {
#if UNITY_ASSERTIONS
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
