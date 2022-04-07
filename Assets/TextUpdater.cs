using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
