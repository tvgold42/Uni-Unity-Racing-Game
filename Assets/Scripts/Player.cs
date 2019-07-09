using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerPos;
    public Rigidbody playerRB;
    public BoxCollider playerCol;
    public float accel;
    public float steer;
    public float angle;
    public float topSpeed;
    public float accelerate;
    public float brake;
    public static float playerX;
    public static float playerZ;

    public float initialSize;

    public GameObject boostEffect;
    public GameObject landEffect;





    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<BoxCollider>();
        initialSize = transform.localScale.x;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerX = gameObject.transform.position.x;
        playerZ = gameObject.transform.position.z;


        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        accelerate = Input.GetAxis("RightTrigger");
        brake = Input.GetAxis("LeftTrigger");
        accelerate -= brake * 0.6f;

        if ( playerPos.localScale.x <= initialSize)
        {
            playerPos.localScale = new Vector3(initialSize, initialSize, initialSize);
        }
        if (playerPos.localScale.x >= initialSize)
        {
            if (playerPos.position.y >= 2)
            {
                playerPos.localScale = new Vector3(playerPos.position.y / 1.3f, playerPos.position.y / 1.3f, initialSize);
            }
        }
        //limit max scale
        if (playerPos.localScale.x >= 5)
        {
            playerPos.localScale = new Vector3(5, 5, initialSize);
        }

        //fall faster
        if (playerRB.velocity.y >= 0 && playerPos.position.y >= 2)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y - Time.deltaTime * 10, playerRB.velocity.z);
        }

        if (Input.GetAxis("Horizontal") < -0.5 || Input.GetKey("left"))
        {
            angle += Time.deltaTime * 250;
        }
        if (Input.GetAxis("Horizontal") > 0.5 || Input.GetKey("right"))
        {
            angle -= Time.deltaTime * 250;
        }
        if (Input.GetAxis("Vertical") != 0 )
        {
            playerRB.AddForce(transform.up * (v * accel));
        }
        if (accelerate != 0)
        {
            playerRB.AddForce(transform.up * (accelerate * accel));

        }
   

        if (playerRB.velocity.x >= topSpeed)
        {
            playerRB.velocity = new Vector3(topSpeed - Time.deltaTime, playerRB.velocity.y, playerRB.velocity.z);
        }
        if (playerRB.velocity.z >= topSpeed)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y, topSpeed - Time.deltaTime);
        }
        if (playerRB.velocity.x <= -topSpeed)
        {
            playerRB.velocity = new Vector3(-topSpeed + Time.deltaTime, playerRB.velocity.y, playerRB.velocity.z);
        }
        if (playerRB.velocity.z <= -topSpeed)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y, -topSpeed + Time.deltaTime);
        }


        transform.eulerAngles = new Vector3(90, transform.rotation.y, angle);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BoostPad" )
        {
            Debug.Log("BoostPad");
            playerRB.AddForce(transform.up * topSpeed * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.5f;
            CameraMovement.shake_decay = 0.005f;

        }


    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground" && playerRB.velocity.y != 0)
        {
            //poomf effect
            Debug.Log("poomf");
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.3f;
            CameraMovement.shake_decay = 0.005f;
        }
    }
}
    

