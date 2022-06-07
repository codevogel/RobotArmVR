using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] phaseMenus;
    
    public void DeactivateWarning()
    {
        foreach (GameObject menu in phaseMenus)
        {
            menu.transform.GetChild(0).gameObject.SetActive(true);
            menu.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
