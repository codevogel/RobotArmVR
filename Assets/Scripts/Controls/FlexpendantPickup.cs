using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexpendantPickup : MonoBehaviour
{

    public GameObject flexPendant;
    public CustomInteractor customInteractor;

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
}
