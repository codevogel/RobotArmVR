using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    [SerializeField] private Transform contentHolder;
    [SerializeField] private GameObject[] phaseMenus;
    
    public void DeactivateWarning()
    {
        foreach (GameObject menu in phaseMenus)
        {
            menu.transform.GetChild(0).gameObject.SetActive(true);
            menu.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ActivatePhaseButton(int buttonNumber)
    {
        if (!contentHolder.GetChild(buttonNumber).gameObject.activeSelf)
        {
            contentHolder.GetChild(buttonNumber).gameObject.SetActive(true);
        }
    }

    public void UnlockAllButtons()
    {
        foreach (Transform child in contentHolder.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
