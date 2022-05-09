using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWriterText : MonoBehaviour
{
    [SerializeField, Range(0.1f, 0.01f)]private float charWaitTime;
    //[SerializeField]private string displayText;
    private TextMeshProUGUI textHolder;

    [ExecuteInEditMode]
    private void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
        textHolder.text = string.Empty;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    textHolder = GetComponent<TextMeshProUGUI>();
    //    textHolder.text = null;
    //    StartCoroutine(TextChange());
    //}

    public void Write(string text)
    {
        textHolder.text = string.Empty;

        StartCoroutine(TextChange(text));
    }

    public void Clear()
    {
        textHolder.text = string.Empty;
    }

    IEnumerator TextChange(string text)
    {
        foreach (char c in text)
        {
            textHolder.text += c;
            yield return new WaitForSeconds(charWaitTime);
        }
    }
}
