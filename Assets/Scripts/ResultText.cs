using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    public Text myText;
    public GameObject resultHandler;
    private double privateTime;


    void Start()
    {
        myText = GetComponent<Text>();
        resultHandler = GameObject.Find("ResultHandler");
        privateTime = resultHandler.GetComponent<ResultHandler>().playerTime;
        privateTime = Math.Round(privateTime, 2);

        if (gameObject.name == "Time")
        {
            myText.text = "Your time: " + privateTime.ToString();
        }

        if (gameObject.name == "PlayerStats")
        {
            myText.text = resultHandler.GetComponent<ResultHandler>().racerPlayerStats[0].ToString()
            + "         " + resultHandler.GetComponent<ResultHandler>().racerPlayerStats[1].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerPlayerStats[2].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerPlayerStats[3].ToString();
        }
        if (gameObject.name == "AIStats1")
        {
            myText.text = resultHandler.GetComponent<ResultHandler>().racerAI1Stats[0].ToString()
            + "         " + resultHandler.GetComponent<ResultHandler>().racerAI1Stats[1].ToString()
            +"          " + resultHandler.GetComponent<ResultHandler>().racerAI1Stats[2].ToString()
            +"          " + resultHandler.GetComponent<ResultHandler>().racerAI1Stats[3].ToString();
        }
        if (gameObject.name == "AIStats2")
        {
            myText.text = resultHandler.GetComponent<ResultHandler>().racerAI2Stats[0].ToString()
            + "         " + resultHandler.GetComponent<ResultHandler>().racerAI2Stats[1].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI2Stats[2].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI2Stats[3].ToString();
        }
        if (gameObject.name == "AIStats3")
        {
            myText.text = resultHandler.GetComponent<ResultHandler>().racerAI3Stats[0].ToString()
            + "         " + resultHandler.GetComponent<ResultHandler>().racerAI3Stats[1].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI3Stats[2].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI3Stats[3].ToString();
        }
        if (gameObject.name == "AIStats4")
        {
            myText.text = resultHandler.GetComponent<ResultHandler>().racerAI4Stats[0].ToString()
            + "         " + resultHandler.GetComponent<ResultHandler>().racerAI4Stats[1].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI4Stats[2].ToString()
            + "          " + resultHandler.GetComponent<ResultHandler>().racerAI4Stats[3].ToString();
        }

    }

}
