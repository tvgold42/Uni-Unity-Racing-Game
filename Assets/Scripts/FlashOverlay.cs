using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOverlay : MonoBehaviour
{
    public Animator overlayAnim;
    public float endTime = 15;

    void Start()
    {
        overlayAnim = GetComponent<Animator>();
        if (gameObject.name == "WhiteFlashNoFade(Clone)" && RaceHandler.raceStarted == true && RaceHandler.raceTime >= 0.5f)
        {
            overlayAnim.SetBool("Boost", true);
            
        }
    }

    void Update()
    {
        endTime -= Time.deltaTime;

        if (endTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
