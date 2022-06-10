using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveColourChanger : MonoBehaviour
{

    private SkinnedMeshRenderer meshRenderer;

    public Color32 gloveColour;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void ChangeColour()
    {
        meshRenderer.material.color = gloveColour;
    }
}
