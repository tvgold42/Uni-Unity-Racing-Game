using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject raceHandler;
    public SpriteRenderer pauseRender;

    // Start is called before the first frame update
    void Start()
    {
        pauseRender = GetComponent<SpriteRenderer>();
        pauseRender.enabled = false;
    }

    // Update is called once per frame
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
