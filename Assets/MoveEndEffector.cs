using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndEffector : MonoBehaviour
{
    public void MoveUp()
    {
        Debug.Log("TEST1");
        transform.position += Vector3.up * Time.deltaTime;

        //for (int i = 1; i > 0; i++)
        //{
        //    Debug.Log("Test2");
        //}

        //float xAs = transform.position.x;
        // for (float i = this.xAs += Vector3.up * Time.deltaTime;
    }

    public void MoveDown()
    {
        transform.position += Vector3.down * Time.deltaTime;
    }

    public void MoveLeft()
    {
        transform.position += Vector3.left * Time.deltaTime;
    }

    public void MoveRight()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
