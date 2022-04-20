using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
	[SerializeField]private float charWaitTime;
  [SerializeField]private string displayText;
  private TextMeshProUGUI textHolder;

    // Start is called before the first frame update
    void Start()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
        textHolder.text = null;
        StartCoroutine(TextChange());
    }

    IEnumerator TextChange()
    {
        foreach (char c in displayText)
        {
            textHolder.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
