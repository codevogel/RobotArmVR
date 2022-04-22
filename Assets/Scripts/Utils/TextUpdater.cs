using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Attach to an object with a TextMeshPro and reference this script to update it's text in code.
/// </summary>
public class TextUpdater : MonoBehaviour
{

    private TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    public void UpdateText(string message)
    {
        textMeshPro.text = message;
    }
}
