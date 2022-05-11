using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private List<PushButton> buttons;

    public void MakeKinematic(bool enable)
    {
        foreach (PushButton button in buttons)
        {
            button.rb.isKinematic = enable;
        }
    }

}
