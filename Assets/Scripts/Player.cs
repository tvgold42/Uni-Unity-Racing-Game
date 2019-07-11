using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerPos;
    public Rigidbody playerRB;
    public BoxCollider playerCol;
    public AudioSource playerSound;
    public AudioClip boostSound;
    public AudioClip tussleSound;
    public float accel;
    public float steer;
    public float angle;
    public float topSpeed;
    public float accelerate;
    public float brake;
    public float collideBoostTimer;
    public static float playerX;
    public static float playerZ;

    public float xVelocity;
    public float zVelocity;

    public float initialSize;

    public GameObject boostEffect;
    public GameObject landEffect;





    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<BoxCollider>();
        playerSound = GetComponent<AudioSource>();
        initialSize = transform.localScale.x;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerX = gameObject.transform.position.x;
        playerZ = gameObject.transform.position.z;
        xVelocity = playerRB.velocity.x;
        zVelocity = playerRB.velocity.z;

        //make it so the vehicle cant spam boost collide with other
        if (collideBoostTimer > 0)
        {
            collideBoostTimer -= Time.deltaTime;
        }


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

        //disable controls if race hasnt started
        if (RaceHandler.raceStarted == true)
        {
            if (Input.GetKey("left"))
            {
                angle += Time.deltaTime * 75 + (h * 1.5f);
            }
            if (Input.GetAxis("Horizontal") < -0.3)
            {
                angle += Time.deltaTime * 125 + (Input.GetAxis("Horizontal") * -5);
            }
            if (Input.GetKey("right"))
            {
                angle -= Time.deltaTime * 75 - (h * 1.5f);
            }
            if (Input.GetAxis("Horizontal") > 0.3)
            {
                angle -= Time.deltaTime * 125 - (Input.GetAxis("Horizontal") * -5);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                playerRB.AddForce(transform.up * (v * accel));
            }
            if (accelerate != 0)
            {
                playerRB.AddForce(transform.up * (accelerate * accel));
            }
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
            playerRB.AddForce(transform.up * 500 * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.5f;
            CameraMovement.shake_decay = 0.005f;
            //change volume accordingly
            playerSound.volume = 0.2f;
            playerSound.PlayOneShot(boostSound, 1f);

        }


    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground" && playerRB.velocity.y != 0)
        {
            //poomf effect
            Debug.Log("poomf");
          //  Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.3f;
            CameraMovement.shake_decay = 0.005f;
        }

        //colliding with other vehicle
        if (other.gameObject.tag == "Vehicle" && collideBoostTimer <= 0)
        {
            other.gameObject.GetComponent<AIEngine>().AIRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f,1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
            //poomf effect
            Debug.Log("Collision with vehicle " + other.gameObject.name);
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            playerSound.volume = 0.2f;
            playerSound.PlayOneShot(tussleSound, 1f);
            collideBoostTimer = 0.7f;
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.3f;
            CameraMovement.shake_decay = 0.005f;
        }
    }
}
    

