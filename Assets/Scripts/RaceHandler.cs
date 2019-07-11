using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceHandler : MonoBehaviour
{
    public float countDown = 5;
    public static bool raceStarted = false;
    public AudioSource raceHandlerAudio;
    public AudioClip countdownAudio;
    public bool countdownAudioPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        raceHandlerAudio = GetComponent<AudioSource>();
        raceStarted = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }

        if (countDown <= 4 && countdownAudioPlayed == false)
        {
            countdownAudioPlayed = true;
            raceHandlerAudio.volume = 0.35f;
            raceHandlerAudio.PlayOneShot(countdownAudio, 1f);
        }
        if (countDown <= 0 && raceStarted == false)
        {
            raceStarted = true;
        }
    }
}
