using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceHandler : MonoBehaviour
{
    public float countDown = 4;
    public static bool raceStarted = false;
    public static bool racePreview = true;
    public static float raceTime;
    public AudioSource raceHandlerAudio;
    public AudioClip countdownAudio;
    public AudioClip trackIntroAudio;
    public AudioClip trackMainAudio;
    public float timeLeft;
    public bool raceFinished;

    public GameObject whiteFlash;
    public GameObject whiteFade;
    // Start is called before the first frame update
    void Start()
    {
        raceHandlerAudio = GetComponent<AudioSource>();
        raceStarted = false;
        racePreview = true;
        raceHandlerAudio.clip = trackIntroAudio;
        raceHandlerAudio.Play();


    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft <= 0 && raceFinished == false)
        {
            raceFinished = true;
            //have some text pop up and then fade out
            Instantiate(whiteFade, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
        }
        if (timeLeft <= -1) { SceneManager.LoadScene("Results"); }
        //race timer
        if (raceStarted == true)
        {
            raceTime += Time.deltaTime;
            timeLeft -= Time.deltaTime;
        }
        if (countDown > 0 && racePreview == false)
        {
            countDown -= Time.deltaTime;
        }

    
        if (countDown <= 0 && raceStarted == false)
        {
            raceStarted = true;
            //flash
            Instantiate(whiteFlash, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
        }
        //skipping track preview
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Submit")) && racePreview == true)
        {
           racePreview = false;
            //stop playing intro music and play main music
            raceHandlerAudio.Stop();
            raceHandlerAudio.clip = trackMainAudio;
            raceHandlerAudio.Play();
            raceHandlerAudio.PlayOneShot(countdownAudio, 1f);
            //flash
            Instantiate(whiteFlash, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90,0,0));
        }
    }
}
