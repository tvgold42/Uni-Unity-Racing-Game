using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrail : MonoBehaviour
{
    private TrailRenderer playerTrail;
    public GameObject vehicle;
    public TrailRenderer playerBoostTrail;
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
        speedX = vehicle.GetComponent<Player>().playerRB.velocity.x;
        speedZ = vehicle.GetComponent<Player>().playerRB.velocity.z;

        if ((speedX >= 15 || speedX <= -15) || Input.GetAxis("Horizontal") >= 0.5f || Input.GetAxis("Horizontal") <= -0.5f)
        {
            playerTrail.emitting = true;
        }

        if ((speedZ >= 15 || speedZ <= -15) || Input.GetAxis("Horizontal") >= 0.5f || Input.GetAxis("Horizontal") <= -0.5f)
        {
            playerTrail.emitting = true;
        }
        if (speedZ <= 15 && speedZ >= -15 && speedX <= 15 && speedX >= -15 && ( Input.GetAxis("Horizontal") <= 0.5f && Input.GetAxis("Horizontal") >= -0.5f) )
        {
            playerTrail.emitting = false;
        }

        //toggle between trails depending on boost
        if (vehicle.GetComponent<Player>().fuelBoosting == true)
        {
            playerBoostTrail.emitting = true;
            playerTrail.emitting = false;
        }

        if (vehicle.GetComponent<Player>().fuelBoosting == false)
        {
            playerBoostTrail.emitting = false;
            playerTrail.emitting = true;
        }
        //stop emitting if dead
        if (vehicle.GetComponent<Player>().currentHealth <= 0)
        {
            playerTrail.emitting = false;
        }
        //stop emitting if dead
        if (vehicle.GetComponent<Player>().currentHealth > 0)
        {
            playerTrail.emitting = true;
        }
    }
}
