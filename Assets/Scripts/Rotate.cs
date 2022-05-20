using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Flexpendant"))
        {
            transform.Rotate(0, 0, 0.75f, Space.Self);
        }

        if (gameObject.CompareTag("Logo"))
        {
            transform.Rotate(0, 0, -0.1f, Space.Self);
        }
    }
}
