using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches the flexpendant to the left hand when needed
/// </summary>
public class FlexpendantPickup : MonoBehaviour
{

    public GameObject flexPendant;
    public CustomInteractor customInteractor;

    /// <summary>
    /// Attaches the flexpendant after touching the hologram
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ControllerLeft"))
        {
            flexPendant.gameObject.SetActive(true);
            flexPendant.transform.parent = other.transform;
            flexPendant.transform.localPosition = other.transform.Find("Flexpendant-offset").localPosition;
            flexPendant.transform.localRotation = other.transform.Find("Flexpendant-offset").localRotation;
            flexPendant.transform.localScale = other.transform.Find("Flexpendant-offset").localScale;
            customInteractor.transform.GetComponent<RobotArmController>().EnableButtons(true);
            customInteractor.OnAttachObject(flexPendant.transform);
        }
    }

    /// <summary>
    /// Attaches the flexpendant after skipping the tutorial
    /// </summary>
    public void SkipTutorial()
    {
        flexPendant.gameObject.SetActive(true);
        flexPendant.transform.parent = HandManager.Instance.LeftController.transform;
        flexPendant.transform.localPosition = HandManager.Instance.LeftController.transform.Find("Flexpendant-offset").localPosition;
        flexPendant.transform.localRotation = HandManager.Instance.LeftController.transform.Find("Flexpendant-offset").localRotation;
        flexPendant.transform.localScale = HandManager.Instance.LeftController.transform.Find("Flexpendant-offset").localScale;
        customInteractor.transform.GetComponent<RobotArmController>().EnableButtons(true);
        customInteractor.OnAttachObject(flexPendant.transform);
    }
}
