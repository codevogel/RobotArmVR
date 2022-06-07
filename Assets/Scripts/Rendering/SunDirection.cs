using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SunDirection : MonoBehaviour
{
    public Color sunColor;

    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
        Shader.SetGlobalColor("_SunColor", sunColor);
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void SetStandardSunColor()
    {
        Shader.SetGlobalVector("_SunDirection", Vector3.right);
        Shader.SetGlobalColor("_SunColor", Color.white);
    }
#endif
}