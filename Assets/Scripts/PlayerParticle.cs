﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    public GameObject vehicle;
    public ParticleSystem vehicleParticle;
    // Start is called before the first frame update
    void Start()
    {
        vehicleParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
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