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
    private static float playerX;
    private static float playerZ;

    public float fuelLeft;
    public bool fuelBoosting;
    public float boostCooldown;
    public float currentLap;
    public float currentCheckPoint;

    private float xVelocity;
    private float zVelocity;

    public float yPositionCap;

    private float initialSize;

    public GameObject boostEffect;
    public GameObject landEffect;

    public GameObject racehandler;





    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<BoxCollider>();
        playerSound = GetComponent<AudioSource>();
        initialSize = transform.localScale.x;
        fuelLeft = 10;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerX = gameObject.transform.position.x;
        playerZ = gameObject.transform.position.z;
        xVelocity = playerRB.velocity.x;
        zVelocity = playerRB.velocity.z;
        if (transform.position.y >= yPositionCap)
        {
            transform.position = new Vector3(transform.position.x, yPositionCap, transform.position.z);
        }

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

            //make this work with a controller too 
            //boosting
            if (Input.GetKey("space") && fuelLeft > 0 && boostCooldown <= 0)
            {
                if (fuelBoosting == false) { Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    //set base boosting velocity
                    playerRB.AddForce(transform.up * accel  * 30);
                    Instantiate(racehandler.GetComponent<RaceHandler>().whiteFlash, new Vector3(racehandler.transform.position.x, racehandler.transform.position.y + 1, racehandler.transform.position.z), Quaternion.Euler(90, 0, 0));
                    fuelBoosting = true;

                }

                fuelLeft -= Time.deltaTime * 3;
                fuelBoosting = true;
            }
            else
                fuelBoosting = false;

            if(Input.GetKeyUp("space") || (Input.GetKey("space") && fuelLeft <= 0))
            {
                CameraMovement.shake_intensity = 0f;
                CameraMovement.originPosition = transform.position;
                //cooldown so you cant spam boost activation
                boostCooldown = 2;

            }

            if (fuelBoosting == true) { accel = 500;
                CameraMovement.shake_intensity = 0.1f;
                CameraMovement.originPosition = transform.position;
            }
            if (fuelBoosting == false) { accel = 330; boostCooldown -= Time.deltaTime; }

            //regen boost when boost cooldown over
            if(boostCooldown <= 0 && fuelLeft <= 5){fuelLeft += Time.deltaTime / 2;}
            //keep fuel at cap
            if(fuelLeft >= 10) { fuelLeft = 10; }
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
            playerRB.AddForce(transform.up * 800 * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 0.5f;
            CameraMovement.shake_decay = 0.005f;
            //change volume accordingly
            playerSound.volume = 0.2f;
            playerSound.PlayOneShot(boostSound, 1f);
            collideBoostTimer = 0.7f;

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
            collideBoostTimer = 0.7f;
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
            fuelLeft += 1;
        }
    }
}
    

