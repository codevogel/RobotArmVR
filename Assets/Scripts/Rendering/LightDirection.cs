using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightDirection : MonoBehaviour
{
    [SerializeField]
    private Color Color;

    private void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
        Shader.SetGlobalVector("_SunMoonColor", Color);
    }
}
