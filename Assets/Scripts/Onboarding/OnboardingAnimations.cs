using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingAnimations : MonoBehaviour
{
    private Animator AnimationManager;
    [SerializeField] private bool isTeleporting;
    [SerializeField] private bool isGrabbing;
    [SerializeField] private bool isGreeting;
    [SerializeField] private bool isPushing;
   
    void Start()
    {
        AnimationManager = GetComponent<Animator>();
        //AnimationManager.SetBool("isGreeting",true);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationManager.SetBool("isTeleporting",isTeleporting);
        AnimationManager.SetBool("isGrabbing",isGrabbing);
        AnimationManager.SetBool("isGreeting",isGreeting);
        AnimationManager.SetBool("isPushing",isPushing);
    }
}
