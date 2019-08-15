using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModel : MonoBehaviour
{
    public GameObject parentVehicle;
    public float baseScale;

    void Start()
    {
        baseScale = transform.localScale.x;
    }

    void Update()
    {
        if (parentVehicle.tag == "Player")
        {
            if (parentVehicle.GetComponent<Player>().currentHealth <= 0)
            {
                transform.localScale = new Vector3(0, 0, 0);
            }
            if (parentVehicle.GetComponent<Player>().currentHealth > 0)
            {
                transform.localScale = new Vector3(baseScale, baseScale, baseScale);
            }
        }
        if (parentVehicle.tag == "Vehicle")
        {
            if (parentVehicle.GetComponent<AIEngine>().currentHealth <= 0)
            {
                transform.localScale = new Vector3(0, 0, 0);
            }
            if (parentVehicle.GetComponent<AIEngine>().currentHealth > 0)
            {
                transform.localScale = new Vector3(baseScale, baseScale, baseScale);
            }
        }
    }
}
