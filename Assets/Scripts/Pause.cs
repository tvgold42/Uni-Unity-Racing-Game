using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject raceHandler;
    public SpriteRenderer pauseRender;


    void Start()
    {
        pauseRender = GetComponent<SpriteRenderer>();
        pauseRender.enabled = false;
    }

    void Update()
    {
        if (raceHandler.GetComponent<RaceHandler>().paused == true)
        {
            pauseRender.enabled = true;
        }
        if (raceHandler.GetComponent<RaceHandler>().paused == false)
        {
            pauseRender.enabled = false;
        }
    }
}
