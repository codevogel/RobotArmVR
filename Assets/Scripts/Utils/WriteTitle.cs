using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Writes the title text 
/// </summary>
public class WriteTitle : MonoBehaviour
{
    private TextMeshProUGUI textHolder;

    // Start is called before the first frame update
    void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Writes the title text
    /// </summary>
    /// <param name="text">The text that needs to be written</param>
    public void WriteText(string text)
    {
        textHolder.text = string.Empty;
        textHolder.text = text;
    }
}
