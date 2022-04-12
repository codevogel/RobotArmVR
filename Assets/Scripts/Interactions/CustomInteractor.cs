using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomInteractor : XRDirectInteractor
{
    public Transform HeldObject { get; set; }

    public void OnAttachObject(SelectEnterEventArgs args)
    {
        HeldObject = args.interactableObject.transform;
    }

    public void OnDetachObject(SelectExitEventArgs args)
    {
        HeldObject = null;
    }

}
