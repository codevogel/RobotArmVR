using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// A mechanism to write text over a period of time in a TMP text field.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class TypeWriterText : MonoBehaviour
{
    /// <summary>
    /// The time in seconds between each character appearing on screen.
    /// </summary>
    [SerializeField, Range(0.1f, 0.01f)] 
    private float charWaitTime;

    private TextMeshProUGUI textHolder;
    private Coroutine writeCoroutine;

    [ExecuteInEditMode]
    private void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
        textHolder.text = string.Empty;
    }

    /// <summary>
    /// Clears the text area and writes down the text over time.
    /// </summary>
    /// <param name="text"> The text to be written.</param>
    public void Write(string text)
    {
        Clear();

        StartCoroutine(TextChange(text));
    }

    /// <summary>
    /// Appends the text to already displayed text in the area.
    /// </summary>
    /// <param name="text"> The text to be written.</param>
    public void Append(string text)
    {
        StartCoroutine(TextChange(text));
    }

    /// <summary>
    /// Stops the active writing coroutine and clears the text area of any text.
    /// </summary>
    public void Clear()
    {
        if (writeCoroutine != null)
            StopCoroutine(writeCoroutine);

        textHolder.text = string.Empty;
    }

    /// <summary>
    /// Appends characters over time to the text area on a set interval.
    /// </summary>
    /// <param name="text"> The text to be written.</param>
    private IEnumerator TextChange(string text)
    {
        foreach (char c in text)
        {
            textHolder.text += c;
            yield return new WaitForSeconds(charWaitTime);
        }
    }
}
