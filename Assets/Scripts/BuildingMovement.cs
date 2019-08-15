using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    //this script is for the environment buildings floating up and down
    public bool movingUpwards = false;
    public float baseHeight;
    public float maxHeight;
    public float minHeight;
    public float heightModifier;

    void Start()
    {
        baseHeight = transform.position.y;
        maxHeight = baseHeight + 5;
        minHeight = baseHeight - 5;
        heightModifier = Random.Range(-0.002f, -0.008f);
    }


    void Update()
    {
            transform.position = new Vector3(transform.position.x, transform.position.y + heightModifier, transform.position.z);

        if (movingUpwards == false && transform.position.y < minHeight)
        {
            movingUpwards = true;
            heightModifier = Random.Range(0.002f, 0.008f);
        }

        if (movingUpwards == true && transform.position.y > maxHeight)
        {
            movingUpwards = false;
            heightModifier = Random.Range(-0.002f, -0.008f);
        }

    }
}
