using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceHud : MonoBehaviour
{
    public SpriteRenderer hudRender;
    public ParticleSystem boostParticle;
    public GameObject player;
    public float baseBarSize;
    public float barSize;

    // Start is called before the first frame update
    void Start()
    {
        baseBarSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //disable hud if race hasnt started
        if (gameObject.tag == "Hud")
        {
            if(RaceHandler.racePreview == false && hudRender.enabled == false){ hudRender.enabled = true; }
            if(RaceHandler.racePreview == true && hudRender.enabled == true) { hudRender.enabled = false; }
        }

        //destroy track preview elements
        if (RaceHandler.racePreview == false && gameObject.tag == "PreviewHud")
        {
            Destroy(gameObject);
        }

        //boost bar size relative to fuel remaining
        if (gameObject.name == "Hud_Boost")
        {
            barSize = baseBarSize * (player.GetComponent<Player>().fuelLeft / 10);
            transform.localScale = new Vector3(barSize,transform.localScale.y,transform.localScale.z);

        }

        //boost bar particle
        if (gameObject.name == "Hud_Boost_Particle")
        {
            if (player.GetComponent<Player>().fuelBoosting == true)
            {
                boostParticle.Play();
            }
            if (player.GetComponent<Player>().fuelBoosting == false)
            {
                boostParticle.Stop();
            }
        }
    }
}
