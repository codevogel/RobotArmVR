using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        Color32 imageColor;
        imageColor = image.color;
        imageColor.a = (byte) (Math.Abs(Mathf.Sin(Time.time)) * 255f);
        image.color = imageColor;
    }
}
