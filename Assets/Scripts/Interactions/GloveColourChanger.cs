using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the color of the gloves
/// </summary>
public class GloveColourChanger : MonoBehaviour
{

    private SkinnedMeshRenderer meshRenderer;

    public Color32 gloveColour;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// Changes the color of the gloves
    /// </summary>
    public void ChangeColour()
    {
        meshRenderer.material.color = gloveColour;
    }
}
