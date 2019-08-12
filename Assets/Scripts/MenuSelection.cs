using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour
{
    //use simple button1/2/3 names so this script can be used across multiple scenes
    public Transform button1;
    public Transform button2;
    public Transform button3;
    public Transform fadeOut;
    public Transform flash;
    public Transform loadingScreen;
    public AudioSource buttonAudio;
    public AudioClip buttonMove;
    public AudioClip buttonSelect;
    public string selectedTrack;
    public int maxButton;
    public int minButton;
    public static int activeButton = 1;
    public bool buttonPressed;
    public bool selected = false;

    public static float timeToProceed = 0;


    // Start is called before the first frame update
    void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
        timeToProceed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(selected == true) { timeToProceed += Time.deltaTime; }

        if (selected == false)
        {
            //moving down menu
            if (Input.GetAxis("Vertical") <= -0.1f && activeButton < maxButton && buttonPressed == false)
            {
                activeButton += 1;
                buttonPressed = true;
                buttonAudio.PlayOneShot(buttonMove, 1f);
            }
            if (Input.GetAxis("Vertical") >= 0.1f && activeButton > minButton && buttonPressed == false)
            {
                activeButton -= 1;
                buttonPressed = true;
                buttonAudio.PlayOneShot(buttonMove, 1f);
            }
            //detect release of button so that holding a button down doesnt make the cursor go straight to the bottom/top
            if (Input.GetAxis("Vertical") >= -0.1f && Input.GetAxis("Vertical") <= 0.1f)
            {
                buttonPressed = false;
            }
        }

        //place highlight in front of appropriate button
        if (activeButton == 1) { transform.position = new Vector3(button1.position.x, button1.position.y + 1, button1.position.z); }
        if (activeButton == 2) { transform.position = new Vector3(button2.position.x, button2.position.y + 1, button2.position.z); }
        if (activeButton == 3) { transform.position = new Vector3(button3.position.x, button3.position.y + 1, button3.position.z); }


        //selecting
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Fire1")) && selected == false)
        {
            Debug.Log("entered");
            selected = true;
            buttonAudio.PlayOneShot(buttonSelect, 1f);
          //  if (SceneManager.GetActiveScene().name != "TrackSelect") { Instantiate(fadeOut, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation); }

            //if selecting track and not back button
            if (SceneManager.GetActiveScene().name == "TrackSelect" && activeButton != maxButton) { Instantiate(flash, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
                Instantiate(loadingScreen, new Vector3(0, transform.position.y + 1, 0), transform.rotation);
                if (activeButton == 1) { selectedTrack = "RingLevel2"; }
                if (activeButton == 2) { selectedTrack = "RingLevel3"; }
                StartCoroutine(SceneLoad());
                //assign track to selectedTrack and start to load it
            }

            else
            Instantiate(fadeOut, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        }
        //press b on controller to go back
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Fire2")) && selected == false && SceneManager.GetActiveScene().name != "Title")
        {
            selected = true;
            buttonAudio.PlayOneShot(buttonSelect, 1f);
            activeButton = maxButton;
            Instantiate(fadeOut, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        }

            //proceeding to relavent scene
            if (timeToProceed >= 1.5f)
        {
            //get scene and then the selection made
            //title
            if (SceneManager.GetActiveScene().name == "Title")
            {
                //go to main menu
                if (activeButton == 1)
                { SceneManager.LoadScene("MainMenu"); }
            }
            //main menu
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                //go to vehicle select
                if (activeButton == 1)
                { SceneManager.LoadScene("TrackSelect"); }
                //quit
                if (activeButton >= 2)
                { Application.Quit();
                    Debug.Log("Game Quit");
                }
            }
            if (SceneManager.GetActiveScene().name == "VehicleSelect")
            {
                if (activeButton == 1)
                {
                    SceneManager.LoadScene("TrackSelect");
                    Player.selectedVehicle = 1;
                }
                if (activeButton == 2)
                {
                    SceneManager.LoadScene("TrackSelect");
                    Player.selectedVehicle = 2;
                }
                if (activeButton == 3)
                { SceneManager.LoadScene("MainMenu"); }
            }
            if (SceneManager.GetActiveScene().name == "TrackSelect")
            {
                if (activeButton == 3)
                { SceneManager.LoadScene("MainMenu"); }
            }

            if (SceneManager.GetActiveScene().name == "Results" || SceneManager.GetActiveScene().name == "GameOver")
            {
                    SceneManager.LoadScene("TrackSelect");
            }


            //longer hold while levels load
            if (timeToProceed >= 8f)
            {
                if (SceneManager.GetActiveScene().name == "TrackSelect")
                {
                    if (activeButton == 1)
                    {
                        SceneManager.LoadScene("RingLevel2");
                    }
                    if (activeButton == 2)
                    {
                        SceneManager.LoadScene("RingLevel3");
                    }
                }
            }
        }


    }


    //load the selected track while the loading screen is there
    private IEnumerator SceneLoad()
    {
        while (timeToProceed <= 5f) {yield return null; }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(selectedTrack);
        while (!asyncLoad.isDone) { yield return null; }
    }
}
