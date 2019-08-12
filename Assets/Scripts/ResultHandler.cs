using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultHandler : MonoBehaviour
{
    //keep track of all main racers and their stats
    public GameObject racerPlayer;
    public GameObject racerAI1;
    public GameObject racerAI2;
    public GameObject racerAI3;
    public GameObject racerAI4;
    public GameObject raceHandler;

    //each racers stats will be saved in an array
    //entry 0 = laps completed
    //entry 1 = kills
    //entry 2 = deaths
    //entry 3 = total ratings
    public int[] racerPlayerStats;
    public float[] racerAI1Stats;
    public float[] racerAI2Stats;
    public float[] racerAI3Stats;
    public float[] racerAI4Stats;
    public float playerTime;
    public double roundedPlayerScore;
    public double roundedAI1Score;
    public double roundedAI2Score;
    public double roundedAI3Score;
    public double roundedAI4Score;
    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Results")
        {
            DontDestroyOnLoad(this.gameObject);
        }
        //reset arrays when going back to any menu
        if (SceneManager.GetActiveScene().name != "Results")
        {
            racerPlayerStats = new int[4];
            racerAI1Stats = new float[4];
            racerAI2Stats = new float[4];
            racerAI3Stats = new float[4];
            racerAI4Stats = new float[4];
            playerTime = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (racerPlayer == null)
        {
            Destroy(gameObject);
        }
        //destory object if out of results
        if (SceneManager.GetActiveScene().name == "VehicleSelect")
        {
            Destroy(gameObject);
        }
        //keep track of all racers laps, kills, death and total ratings
        if (SceneManager.GetActiveScene().name != "Results")
        {
            playerTime = raceHandler.GetComponent<RaceHandler>().playerTime;

            racerPlayerStats[0] = racerPlayer.GetComponent<Player>().placement;
            racerPlayerStats[1] = racerPlayer.GetComponent<Player>().kills;
            racerPlayerStats[2] = racerPlayer.GetComponent<Player>().deaths;
            racerPlayerStats[3] = racerPlayer.GetComponent<Player>().ratings;

            racerAI1Stats[0] = racerAI1.GetComponent<AIEngine>().placement;
            racerAI1Stats[1] = racerAI1.GetComponent<AIEngine>().kills;
            racerAI1Stats[2] = racerAI1.GetComponent<AIEngine>().deaths;
            racerAI1Stats[3] = racerAI1.GetComponent<AIEngine>().ratings;


            racerAI2Stats[0] = racerAI2.GetComponent<AIEngine>().placement;
            racerAI2Stats[1] = racerAI2.GetComponent<AIEngine>().kills;
            racerAI2Stats[2] = racerAI2.GetComponent<AIEngine>().deaths;
            racerAI2Stats[3] = racerAI2.GetComponent<AIEngine>().ratings;

            racerAI3Stats[0] = racerAI3.GetComponent<AIEngine>().placement;
            racerAI3Stats[1] = racerAI3.GetComponent<AIEngine>().kills;
            racerAI3Stats[2] = racerAI3.GetComponent<AIEngine>().deaths;
            racerAI3Stats[3] = racerAI3.GetComponent<AIEngine>().ratings;

            racerAI4Stats[0] = racerAI4.GetComponent<AIEngine>().placement;
            racerAI4Stats[1] = racerAI4.GetComponent<AIEngine>().kills;
            racerAI4Stats[2] = racerAI4.GetComponent<AIEngine>().deaths;
            racerAI4Stats[3] = racerAI4.GetComponent<AIEngine>().ratings;
        }

    }
}
