using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingHeight;
    [SerializeField] private float bobbingSpeed;
    [SerializeField] private bool rotateZ;

    private float timePassed;
    private float initYPosition;

    private void Start()
    {
        initYPosition = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime * bobbingSpeed;
        float yChange = Mathf.Sin(timePassed) / bobbingHeight;
        transform.position = new Vector3(transform.position.x, yChange + initYPosition, transform.position.z);

        if (rotateZ)
        {
            transform.Rotate(0, 0, -rotateSpeed, Space.Self);
            return;
        }
        
        transform.Rotate(rotateSpeed, 0, 0, Space.Self);
    }
}
