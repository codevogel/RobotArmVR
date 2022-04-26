using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SliderChanger : MonoBehaviour
{

    public ArmBoundsCheck armBoundsCheck;

    public Slider slider1, slider2, slider3;

    public float speed;

    public bool freeMovement;

    private void Update()
    {
        freeMovement = armBoundsCheck.FreeMovement;
        if (armBoundsCheck.FreeMovement)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                slider3.value += 1f * speed;
            }
            if (Keyboard.current.sKey.isPressed)
            {
                slider3.value -= 1f * speed;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                slider1.value += 1f * speed;
            }
            if (Keyboard.current.aKey.isPressed)
            {
                slider1.value -= 1f * speed;
            }

            if (Keyboard.current.qKey.isPressed)
            {
                slider2.value += 1f * speed;
            }
            if (Keyboard.current.eKey.isPressed)
            {
                slider2.value -= 1f * speed;
            }
        }
        else
        {
            if (slider1.value > 0)
            {
                if (Keyboard.current.aKey.isPressed)
                {
                    slider1.value -= 1f * speed;
                }

            }
            else if (Keyboard.current.dKey.isPressed)
            {
                slider1.value += 1f * speed;
            }

            if (slider3.value > 0)
            {
                if (Keyboard.current.sKey.isPressed)
                {
                    slider3.value -= 1f * speed;
                }
            }
            else if (Keyboard.current.wKey.isPressed)
            {
                slider3.value += 1f * speed;
            }

            if (slider2.value > 0)
            {
                if (Keyboard.current.eKey.isPressed)
                {
                    slider2.value -= 1f * speed;
                }
            }
            else
                if (Keyboard.current.qKey.isPressed)
                {
                    slider2.value += 1f * speed;
                }
            }
        }
}
