using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that is used to pass Unity Events to DisplayCaseOpener
/// </summary>
[RequireComponent(typeof(DisplayCaseOpener))]
public class DisplayCaseOpen : MonoBehaviour
{

    private DisplayCaseOpener opener;

    private void Start()
    {
        opener = GetComponent<DisplayCaseOpener>();
    }

    public void Open()
    {
        if (opener != null)
        {
            opener.Open();
        }
    }

}
