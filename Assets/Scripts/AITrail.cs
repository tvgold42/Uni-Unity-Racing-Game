using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrail : MonoBehaviour
{
    private TrailRenderer playerTrail;
    public TrailRenderer boostTrail;
    public GameObject vehicle;
    public float speedX;
    public float speedZ;
    // Start is called before the first frame update
    void Start()
    {
        playerTrail = GetComponent<TrailRenderer>();
        playerTrail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        speedX = vehicle.GetComponent<AIEngine>().AIRB.velocity.x;
        speedZ = vehicle.GetComponent<AIEngine>().AIRB.velocity.z;

        if ((speedX >= 20 || speedX <= -20))
        {
            playerTrail.emitting = true;
        }

        if ((speedZ >= 20 || speedZ <= -20) )
        {
            playerTrail.emitting = true;
        }
        if (speedZ <= 15 && speedZ >= -15 && speedX <= 15 && speedX >= -15 )
        {
            playerTrail.emitting = false;
        }

        //toggle between trails depending on boost
        if (vehicle.GetComponent<AIEngine>().boosting == true)
        {
            boostTrail.emitting = true;
            playerTrail.emitting = false;
        }

        if (vehicle.GetComponent<AIEngine>().boosting == false)
        {
            boostTrail.emitting = false;
            playerTrail.emitting = true;
        }
    }
}
