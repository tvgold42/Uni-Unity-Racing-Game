using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandlerMenu : MonoBehaviour
{ 
    public AudioSource menuAudio;
    // Start is called before the first frame update
    void Awake()
    {
        menuAudio = GetComponent<AudioSource>();
        //dont destroy this object if on any menu scene
        //or just check if its not on any race track
        if (SceneManager.GetActiveScene().name != "RingLevel2" && SceneManager.GetActiveScene().name != "RingLevel3")
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //fade out music if about to start race
        if (SceneManager.GetActiveScene().name == "TrackSelect" && MenuSelection.activeButton !=3 && MenuSelection.timeToProceed > 0)
        {
            menuAudio.volume -= Time.deltaTime;
            Debug.Log("fading out music");
        }
        //destory object if not on the menus anymore
        if (SceneManager.GetActiveScene().name == "RingLevel2" || SceneManager.GetActiveScene().name == "RingLevel3")
        {
            Debug.Log("music handler gone");
            Destroy(gameObject);
        }
    }
}
