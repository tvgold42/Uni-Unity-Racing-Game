using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOverlay : MonoBehaviour
{
    public Animator overlayAnim;
    public float endTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        overlayAnim = GetComponent<Animator>();
        if (gameObject.name == "WhiteFlashNoFade(Clone)" && RaceHandler.raceStarted == true && RaceHandler.raceTime >= 0.5f)
        {
            overlayAnim.SetBool("Boost", true);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        endTime -= Time.deltaTime;

        if (endTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
