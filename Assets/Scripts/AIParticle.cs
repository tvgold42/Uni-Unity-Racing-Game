using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParticle : MonoBehaviour
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
        if (vehicle.GetComponent<AIEngine>().boosting == true)
        {
            vehicleParticle.Play();

        }

        if (vehicle.GetComponent<AIEngine>().boosting == false)
        {
            vehicleParticle.Stop();
        }
    }
}
