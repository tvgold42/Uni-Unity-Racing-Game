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
    public float playerTime;
    public AudioSource raceHandlerAudio;
    public AudioClip countdownAudio;
    public AudioClip trackIntroAudio;
    public AudioClip trackMainAudio;
    public AudioClip victoryAudio;
    public float timeLeft;
    public bool raceFinished;
    public bool resultTransition;

    public GameObject whiteFlash;
    public GameObject whiteFade;
    public GameObject playerObject;
    public GameObject trophyObject;
    // Start is called before the first frame update
    void Start()
    {
        raceHandlerAudio = GetComponent<AudioSource>();
        raceStarted = false;
        racePreview = true;
        raceHandlerAudio.clip = trackIntroAudio;
        raceHandlerAudio.Play();
        resultTransition = false;
        countDown = 4;
        playerTime = 0;


}

    // Update is called once per frame
    void Update()
    {
        if(timeLeft <= 0 && raceFinished == false)
        {
            raceFinished = true;
            racePreview = true;
            //if player is 1st, make trophy appear
            if (playerObject.GetComponent<Player>().placement == 1)
            {
                Instantiate(whiteFlash, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
                trophyObject.GetComponent<SpriteRenderer>().enabled = true;
                raceHandlerAudio.Stop();
                raceHandlerAudio.clip = victoryAudio;
                raceHandlerAudio.Play();
            }
            //have some text pop up and then fade out
            else if (playerObject.GetComponent<Player>().placement != 1)
            {
                Instantiate(whiteFade, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
                resultTransition = true;
            }
        }
        if (timeLeft <= -1 && resultTransition == true && playerObject.GetComponent<Player>().placement == 1) { SceneManager.LoadScene("Results"); }
        if (timeLeft <= -1 && resultTransition == true && playerObject.GetComponent<Player>().placement != 1) { SceneManager.LoadScene("Results"); }
        if (raceFinished == true && resultTransition == false) { timeLeft += Time.deltaTime; }
        //race timer
        if (raceStarted == true && raceFinished == false)
        {
            raceTime += Time.deltaTime;
            timeLeft -= Time.deltaTime;
            playerTime += Time.deltaTime;
        }
        if (raceFinished == true) { timeLeft -= Time.deltaTime; }

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
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")) && racePreview == true)
        {
            if (raceFinished == true)
            {
                Instantiate(whiteFade, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
                resultTransition = true;
            }
            else if (raceFinished == false)
            {
                racePreview = false;
                //stop playing intro music and play main music
                raceHandlerAudio.Stop();
                raceHandlerAudio.clip = trackMainAudio;
                raceHandlerAudio.Play();
                raceHandlerAudio.PlayOneShot(countdownAudio, 1f);
                //flash
                Instantiate(whiteFlash, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
            }
   
        }
    }
}
