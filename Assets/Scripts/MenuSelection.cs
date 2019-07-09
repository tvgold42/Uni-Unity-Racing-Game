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
    public int maxButton;
    public int minButton;
    public int activeButton = 1;
    public bool buttonPressed;
    public bool selected = false;

    public float timeToProceed = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selected == true) { timeToProceed += Time.deltaTime; }

        //moving down menu
        if (Input.GetAxis("Vertical") <= -0.1f && activeButton < maxButton && buttonPressed == false)
        {
            activeButton += 1;
            buttonPressed = true;
        }
        if (Input.GetAxis("Vertical") >= 0.1f && activeButton > minButton && buttonPressed == false)
        {
            activeButton -= 1;
            buttonPressed = true;
        }
        //detect release of button so that holding a button down doesnt make the cursor go straight to the bottom/top
        if (Input.GetAxis("Vertical") >= -0.1f && Input.GetAxis("Vertical") <= 0.1f)
        {
            buttonPressed = false;
        }

        //place highlight in front of appropriate button
        if (activeButton == 1) { transform.position = new Vector3(button1.position.x, button1.position.y + 1, button1.position.z); }
        if (activeButton == 2) { transform.position = new Vector3(button2.position.x, button2.position.y + 1, button2.position.z); }
        if (activeButton == 3) { transform.position = new Vector3(button3.position.x, button3.position.y + 1, button3.position.z); }


        //selecting
        if (Input.GetKeyDown(KeyCode.Return) && selected == false)
        {
            Debug.Log("entered");
            selected = true;
            Instantiate(fadeOut, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        }

        //proceeding to relavent scene
        if (timeToProceed >= 1.5f)
        {
            //go to race
            SceneManager.LoadScene("RingLevel1");
           


        }

    }
}
