using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceHudTime : MonoBehaviour
{
    public Text hudText;
    public GameObject raceHandler;
    private double privateTime;

    void Start()
    {
        hudText = GetComponent<Text>();
    }

    void Update()
    {
        //round time to 2 decimal places, so we only see whole number
        privateTime = raceHandler.GetComponent<RaceHandler>().timeLeft;
        privateTime = Math.Round(privateTime, 1);

        //timer
        if (gameObject.name == "HudTime") { hudText.text = privateTime.ToString(); }
    }
}
