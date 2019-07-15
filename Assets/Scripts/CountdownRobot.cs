using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownRobot : MonoBehaviour
{
    public Animator countdownAnim;
    public bool animationStart = false;
    // Start is called before the first frame update
    void Start()
    {
        countdownAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animationStart == false && RaceHandler.racePreview == false)
        {
            animationStart = true;
            countdownAnim.SetBool("PreviewEnd", true);
        }
    }
}
