using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownRobot : MonoBehaviour
{
    //script for the countdown robots animation at the start of every race
    public Animator countdownAnim;
    public bool animationStart = false;
    
    void Start()
    {
        countdownAnim = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        if (animationStart == false && RaceHandler.racePreview == false)
        {
            animationStart = true;
            countdownAnim.SetBool("PreviewEnd", true);
        }
    }
}
