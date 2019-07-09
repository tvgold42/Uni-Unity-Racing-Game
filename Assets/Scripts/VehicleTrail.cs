using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrail : MonoBehaviour
{
    private TrailRenderer playerTrail;
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
    }
}
