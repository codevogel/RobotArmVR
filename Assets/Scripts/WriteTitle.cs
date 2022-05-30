using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WriteTitle : MonoBehaviour
{
    private TextMeshProUGUI textHolder;

    // Start is called before the first frame update
    void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteText(string text)
    {
        textHolder.text = string.Empty;
        textHolder.text = text;
    }
}
