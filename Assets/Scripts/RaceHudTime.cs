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
    public GameObject playerObject;

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
        if (gameObject.name == "HudLap" && RaceHandler.raceStarted == true) { hudText.text = playerObject.GetComponent<Player>().currentLap.ToString(); }
        if (gameObject.name == "HudPlacement" && RaceHandler.raceStarted == true) { hudText.text = playerObject.GetComponent<Player>().placement.ToString(); }
    }
}
