using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    public GameObject vehicle;
    public ParticleSystem vehicleParticle;

    void Start()
    {
        vehicleParticle = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        //toggle between trails depending on boost
        if (vehicle.GetComponent<Player>().fuelBoosting == true)
        {
            vehicleParticle.Play();

        }

        if (vehicle.GetComponent<Player>().fuelBoosting == false)
        {
            vehicleParticle.Stop();
        }
    }
}
