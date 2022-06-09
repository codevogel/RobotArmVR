using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCaseOpen : MonoBehaviour
{

    private DisplayCaseOpener opener;

    public void Open()
    {
        if (opener != null)
        {
            opener.Open();
        }
    }

}
