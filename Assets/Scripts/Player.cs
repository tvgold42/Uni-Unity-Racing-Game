using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform playerPos;
    public Rigidbody playerRB;
    private BoxCollider playerCol;
    private SpriteRenderer playerRender;
    public AudioSource playerSound;
    public AudioClip boostSound;
    public AudioClip tussleSoundGood;
    public AudioClip tussleSoundBad;
    public AudioClip engineSound;
    public AudioClip engineBoost;
    public AudioClip explosion;
    public float accel;
    public float steer;
    public float angle;
    public float topSpeed;
    public float accelerate;
    public float brake;
    public Vector3 targetRotation;
    public Vector3 newDir;
    private float collideBoostTimer;
    private static float playerX;
    private static float playerZ;

    public float fuelLeft;
    public bool fuelBoosting;
    public float boostCooldown;

    private float xVelocity;
    private float zVelocity;

    public float yPositionCap;

    private float initialSize;

    public float maxHealth;
    public float currentHealth;
  //  private float healthCooldown;
    public int ratings = 0;

    private bool death = false;
    private bool fallingDeath = false;
    private float respawnTimer = 2;
    private float respawnInvuln = 0;
    private bool engineActive = false;

    public GameObject boostEffect;
    public GameObject landEffect;
    public GameObject hurtEffect;
    public GameObject explosionEffect;

    public GameObject racehandler;

    //path finding
    public float currentLap;
    public Transform PlayerPath;
    private List<Transform> pathNodes;
    public int currentPathNode = 0;

    //position tracking
    public int placement;
    public GameObject opponent1;
    public GameObject opponent2;
    public GameObject opponent3;
    public GameObject opponent4;

    //selected vehicle
    public static int selectedVehicle = 1;
    public Sprite vehicle1;
    public Sprite vehicle2;

    //statistics
    public int kills;
    public int deaths;




    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<BoxCollider>();
        playerRender = GetComponent<SpriteRenderer>();
        playerSound = GetComponent<AudioSource>();
        initialSize = transform.localScale.x;
        fuelLeft = 10;
        currentHealth = 10;
        maxHealth = currentHealth;
        ratings = 0;
        currentLap = 0;
        kills = 0;
        deaths = 0;
        Transform[] pathLine = PlayerPath.GetComponentsInChildren<Transform>();
        pathNodes = new List<Transform>();
        //count all the nodes so a lap can be cleared
        for (int i = 0; i < pathLine.Length; i++)
        {
            //not the parent object node though
            if (pathLine[i] != PlayerPath.transform)
            {
                pathNodes.Add(pathLine[i]);
            }
        }

        //use selected vehicle
        if (selectedVehicle == 2)
        {
            playerRender.sprite = vehicle2;
        }


    }


    private void CheckNodeDistance()
    {
        //check if close enough to node to confirm progress through track
        if (Vector3.Distance(transform.position, pathNodes[currentPathNode].position) <= 18f)
        {
            //confirm node is passed and set target to next node
            if (currentPathNode == pathNodes.Count - 1)
            {
                //new lap if at last node
                currentPathNode = 0;

            }
            else
            {
                //if crossing the first point, increase lap count by 1
                if (currentPathNode == 0)
                {
                    currentLap++;
                    //get points
                    ratings += 100;
                }
                //if not at end, go to next node
                currentPathNode++;

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckNodeDistance();

        playerX = gameObject.transform.position.x;
        playerZ = gameObject.transform.position.z;
        xVelocity = playerRB.velocity.x;
        zVelocity = playerRB.velocity.z;
        //always round ratings to the nearest whole number
        Mathf.Round(ratings);

        //health regen cooldown, currently disabled
        /*
        healthCooldown -= Time.deltaTime;
        if (healthCooldown <= 0 && currentHealth <= maxHealth && death == false)
        {
            currentHealth += Time.deltaTime / 3;
        }
        */
        if (currentHealth <= 0) { currentHealth = 0; }

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
        if (RaceHandler.raceStarted == true && death == false && racehandler.GetComponent<RaceHandler>().raceFinished == false)
        {
            if (!Input.GetKey("left") && !Input.GetKey("right"))
            {
                angle += h * 3;
                angle += Input.GetAxis("Horizontal");
            }
            //keyboard
            if (Input.GetKey("left"))
            {
                angle += Time.deltaTime * 50 + (h * 0.5f);
            }
            if (Input.GetKey("right"))
            {
                angle -= Time.deltaTime * 50 - (h * 0.5f);
            }
            //controller
            if (Input.GetAxis("Horizontal") < -0.3)
            {
                angle += Time.deltaTime * 2 + (Input.GetAxis("Horizontal") * -5);
            }
            if (Input.GetAxis("Horizontal") > 0.3)
            {
                angle -= Time.deltaTime * 2 - (Input.GetAxis("Horizontal") * -5);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                playerRB.AddForce(transform.up * (v * accel));
                if (RaceHandler.raceStarted == true && engineActive == false)
                {
                    engineActive = true;
                    playerSound.PlayOneShot(engineSound, 1f);
                }
            }
            if (accelerate != 0)
            {
                playerRB.AddForce(transform.up * (accelerate * accel));
                if (RaceHandler.raceStarted == true && engineActive == false)
                {
                    engineActive = true;
                    playerSound.PlayOneShot(engineSound, 1f);
                }
            }

            //make this work with a controller too 
            //boosting
            if ((Input.GetKey("space") || Input.GetButton("Fire1")) && fuelLeft > 1.5f && boostCooldown <= 0)
            {
                if (fuelBoosting == false) { Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    //set base boosting velocity
                    playerRB.AddForce(transform.up * accel  * 30);
                    Instantiate(racehandler.GetComponent<RaceHandler>().whiteFlash, new Vector3(racehandler.transform.position.x, racehandler.transform.position.y + 1, racehandler.transform.position.z), Quaternion.Euler(90, 0, 0));
                    fuelBoosting = true;
                    //take initial fuel cost
                    fuelLeft -= 1.5f;
                    playerSound.PlayOneShot(engineBoost, 1f);

                }

                fuelLeft -= Time.deltaTime * 3;
                fuelBoosting = true;
            }
            else
                fuelBoosting = false;

            if(Input.GetKeyUp("space") || Input.GetButtonUp("Fire1") || ((Input.GetKey("space") || Input.GetButton("Fire1"))) && fuelLeft <= 0)
            {
                CameraMovement.shake_intensity = 0f;
                CameraMovement.originPosition = transform.position;
                //cooldown so you cant spam boost activation
                boostCooldown = 0.8f;

            }

            if (fuelBoosting == true) { accel = 500;
                CameraMovement.shake_intensity = 0.1f;
                CameraMovement.originPosition = transform.position;
            }
            if (fuelBoosting == false) { accel = 330; boostCooldown -= Time.deltaTime; }

            //boost regen
            if(boostCooldown <= 0){fuelLeft += Time.deltaTime / 4;}

            //keep fuel at cap
            if(fuelLeft >= 10) { fuelLeft = 10; }
        }


        //death and respawning
        if (currentHealth <= 0 && death == false)
        {
            //stop moving, go invisible, spawn explosion prefab
            death = true;
            engineActive = false;
            fuelBoosting = false;
            fuelLeft = 0;
            respawnTimer = 3;
            playerRB.velocity = Vector3.zero;
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 1.25f;
            CameraMovement.shake_decay = 0.005f;
            Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(90,0,0));
            playerSound.PlayOneShot(explosion, 1f);
            //replace sprite renderer with mesh renderer in future
            playerRender.enabled = false;
        }
        if (death == true && respawnTimer >= 0)
        {
            respawnTimer -= Time.deltaTime;
            playerRB.velocity = Vector3.zero;
            playerCol.enabled = false;

            //rotate to face correct way, uses a modified version of the AI's steering function
           // angle = 0;
            Vector3 relativeAngle = transform.InverseTransformPoint(pathNodes[currentPathNode].position);
            float newSteerAngle;
            float maxSteerAngle = 40f;
            newSteerAngle = (relativeAngle.x / relativeAngle.magnitude) * maxSteerAngle;
            transform.eulerAngles = new Vector3(90, transform.rotation.y, angle);
            angle -= newSteerAngle;

        }
        if (death == true && respawnTimer <= 0)
        {
            //respawn at location of checkpoint if fallen off track
            if (fallingDeath == true)
            {
                fallingDeath = false;
                if (currentPathNode > 0)
                { transform.position = pathNodes[currentPathNode - 1].position; }
                if (currentPathNode == 0)
                { transform.position = pathNodes[currentPathNode].position; }                
            }
            Debug.Log("Respawned Player");
            death = false;
            playerCol.enabled = true;
            playerRender.enabled = true;
            respawnInvuln = 3;
            currentHealth = maxHealth;         
        }

        if (respawnInvuln >= 0)
        {
            respawnInvuln -= Time.deltaTime;
            //flash for invuln
            if (respawnInvuln >= 1.5 && respawnInvuln <= 2.5) {playerRender.enabled = false;}
            if (respawnInvuln <= 1.5 && respawnInvuln >= 1) { playerRender.enabled = true; }
            if (respawnInvuln <= 1 && respawnInvuln >= 0.1) { playerRender.enabled = false; }
            if (respawnInvuln <= 0.1) { playerRender.enabled = true; }
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

        //only use angle variable as rotation if alive
        if (currentHealth > 0)
        {
            transform.eulerAngles = new Vector3(90, transform.rotation.y, angle);
        }


        //placement tracking
        //compare lap and checkpoint with each opponent racer
        //if ahead of the racer, move up one place and move them down one place
        if (currentLap <= 5 && currentPathNode != 0 && currentPathNode != pathNodes.Count)
        {
            //opponent1
            if ((currentLap > opponent1.GetComponent<AIEngine>().currentLap) || currentLap == opponent1.GetComponent<AIEngine>().currentLap && currentPathNode > opponent1.GetComponent<AIEngine>().currentPathNode)
            {
                if (opponent1.GetComponent<AIEngine>().placement <= placement && opponent1.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent1.GetComponent<AIEngine>().placement += 1;
                }
            }
            //opponent2
            if ((currentLap >= opponent2.GetComponent<AIEngine>().currentLap) || currentLap > opponent2.GetComponent<AIEngine>().currentLap && currentPathNode > opponent2.GetComponent<AIEngine>().currentPathNode)
            {
                if (opponent2.GetComponent<AIEngine>().placement <= placement && opponent2.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent2.GetComponent<AIEngine>().placement += 1;
                }
            }
            //opponent3
            if ((currentLap >= opponent3.GetComponent<AIEngine>().currentLap) || currentLap > opponent3.GetComponent<AIEngine>().currentLap && currentPathNode > opponent3.GetComponent<AIEngine>().currentPathNode )
            {
                if (opponent3.GetComponent<AIEngine>().placement <= placement && opponent3.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent3.GetComponent<AIEngine>().placement += 1;
                }
            }
            //opponent4
            if ((currentLap >= opponent4.GetComponent<AIEngine>().currentLap) || currentLap > opponent4.GetComponent<AIEngine>().currentLap && currentPathNode > opponent4.GetComponent<AIEngine>().currentPathNode)
            {
                if (opponent4.GetComponent<AIEngine>().placement <= placement && opponent4.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent4.GetComponent<AIEngine>().placement += 1;
                }
            }
        }
        //end race once player reaches lap 4
        if (currentLap == 4)
        {
            if (racehandler.GetComponent<RaceHandler>().timeLeft > 0)
            { racehandler.GetComponent<RaceHandler>().timeLeft = 0; }
            angle += Time.deltaTime * 60;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BoostPad" )
        {
            Debug.Log("BoostPad");
            playerRB.AddForce(transform.up * 800 * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            CameraMovement.originPosition = transform.position;
            CameraMovement.shake_intensity = 1f;
            CameraMovement.shake_decay = 0.005f;
            //change volume accordingly
          //  playerSound.volume = 0.2f;
            playerSound.PlayOneShot(boostSound, 1f);
            collideBoostTimer = 0.7f;
        }

        if (other.gameObject.tag == "ShortcutTrigger")
        {
            Debug.Log("shortcut");
            if (currentPathNode >= 17) { currentPathNode = 25; }

        }

        //pickups
        if (other.gameObject.tag == "HealthPickup")
        {
            if (other.GetComponent<Pickup>().pickupAble == true && currentHealth < maxHealth)
            {
                //restore 20% hp
                currentHealth += maxHealth / 5;
                other.GetComponent<Pickup>().respawnTimer = 4;
                other.GetComponent<Pickup>().playSound = true;
                if (currentHealth >= maxHealth) { currentHealth = maxHealth; }
                
            }
        }
        if (other.gameObject.tag == "BoostPickup")
        {
            if (other.GetComponent<Pickup>().pickupAble == true && fuelLeft < 10)
            {
                //restore 25% fuel
                fuelLeft += 2.5f;
                other.GetComponent<Pickup>().respawnTimer = 4;
                other.GetComponent<Pickup>().playSound = true;
            }
        }

        if (other.gameObject.tag == "KillGrid" && fallingDeath == false)
        {
                //kill the player instantly
                deaths += 1;
                ratings /= 2;
                currentHealth -= 1000;
                fallingDeath = true;
                //prevent massive screen shake glitch
                if (fuelBoosting == true)
                {
                    fuelBoosting = false;
                    CameraMovement.shake_intensity = 0f;
                    CameraMovement.originPosition = transform.position;
                    fuelLeft = 0;
                }
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

        if (currentLap != 4)
        {
            //colliding with other vehicle
            if ((other.gameObject.tag == "Vehicle") && collideBoostTimer <= 0)
            {
                other.gameObject.GetComponent<AIEngine>().AIRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f, 1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
                //poomf effect
                Debug.Log("Collision with vehicle " + other.gameObject.name);

                CameraMovement.originPosition = transform.position;
                CameraMovement.shake_intensity += 0.3f;
                CameraMovement.shake_decay = 0.005f;
                fuelLeft += 1;

                //take damage if not boosting
                if (fuelBoosting == false)
                {
                    //neative collision
                    Instantiate(hurtEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    //only lose health if not just respawned
                    if (respawnInvuln <= 0)
                    {
                        currentHealth -= 4.5f;
                        /*healthCooldown = 2;*/
                    }
                    playerSound.PlayOneShot(tussleSoundBad, 1f);
                    if (currentHealth <= 0)
                    {
                        if (other.gameObject.tag == "Vehicle")
                        {
                            //give opposing vehicle a quarter of their ratings and 1 kill
                            other.gameObject.GetComponent<AIEngine>().ratings += ratings / 4;
                            other.gameObject.GetComponent<AIEngine>().kills += 1;
                            deaths += 1;

                        }
                        ratings /= 2;
                    }
                }
                if (fuelBoosting == true)
                {
                    //positive collision
                    Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    // playerSound.volume = 0.2f;
                    playerSound.PlayOneShot(tussleSoundGood, 1f);
                }
                collideBoostTimer = 0.3f;
            }
        }
    }
}
    

