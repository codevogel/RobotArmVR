using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the skybox material
/// </summary>
public class SkyboxRotation : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time*scrollSpeed);
    }
}
