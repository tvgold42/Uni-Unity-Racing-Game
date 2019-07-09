using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underglow : MonoBehaviour
{
    public Light underLight;
    public GameObject vehicle;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        underLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

        speed = vehicle.GetComponent<Player>().playerRB.velocity.x + vehicle.GetComponent<Player>().playerRB.velocity.z;

        if (speed < 0)
        {
            speed *= -1;
        }
        underLight.intensity = speed;
    }
}
