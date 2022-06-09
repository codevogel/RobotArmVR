using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] phaseMenus;
    [SerializeField] private Transform[] contentHolders;

    public static PhaseChanger Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Deactivates the initial warning on the phase changing menu
    /// </summary>
    public void DeactivateWarning()
    {
        foreach (GameObject menu in phaseMenus)
        {
            menu.transform.GetChild(0).gameObject.SetActive(true);
            menu.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activate a specific button on the phase changing menu
    /// </summary>
    /// <param name="buttonNumber">The number for the button that will be activated</param>
    public void ActivatePhaseButton(int buttonNumber)
    {
        foreach (Transform contentHolder in contentHolders)
        {
            if (!contentHolder.GetChild(buttonNumber).gameObject.activeSelf)
            {
                contentHolder.GetChild(buttonNumber).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Unlock all buttons on the phase changing menu
    /// </summary>
    public void UnlockAllButtons()
    {
        foreach (Transform content in contentHolders)
        {
            foreach (Transform child in content)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Disables the button after a button press
    /// </summary>
    public void LockButtons()
    {
        foreach (Transform content in contentHolders)
        {
            foreach (Transform child in content)
            {

            }
        }
    }
}