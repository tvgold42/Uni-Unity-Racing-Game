using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine : MonoBehaviour
{

    public Transform AIPath;
    public Transform AIPos;
    public Transform playerPos;
    public  Rigidbody AIRB;
    public WheelCollider AIWheel;
    public WheelCollider AIWheel2;
    public SpriteRenderer AIRender;
    public BoxCollider AICol;
    public float maxSteerAngle = 40f;
    public float topSpeed = 500;
    public float topSpeedBackup;
    public float accel;
    public float backupAccel;
    public float newSteerAngle;
    private float oldSteerAngle;
    public float angle;
    public float initialSize;
    public float collideBoostTimer;
    public float boostCoolDown;
    public bool boosting;
    public float xVelocity;
    public float zVelocity;

    public float yPositionCap;

    public float swerveTimer = 8;
    public float swerveAngle;

    public float maxHealth;
    public float currentHealth;
    public int ratings = 0;

    public bool death = false;
    public bool fallingDeath = false;
    public float respawnTimer = 2;
    public float respawnInvuln = 0;

    public float currentLap;
    public float currentCheckPoint;

    public GameObject boostEffect;
    public GameObject landEffect;
    public GameObject explosionEffect;

    private List<Transform> pathNodes;

    public int currentPathNode = 0;


    //placement against opponents
    public int placement;
    //opponent 1 will be the player, using the playerPos variable
    public GameObject opponent2;
    public GameObject opponent3;
    public GameObject opponent4;


    //statistics
    public float kills;
    public float deaths;


    // Start is called before the first frame update
    void Start()
    {
        AIPos = GetComponent<Transform>();
        AIRB = GetComponent<Rigidbody>();
        AICol = GetComponent<BoxCollider>();
        initialSize = transform.localScale.x;
        oldSteerAngle = maxSteerAngle;
        currentHealth = 10;
        maxHealth = currentHealth;
        //fodder enemies have 1 hp
        if (gameObject.tag == "Vehicle Fodder") { currentHealth = 1; maxHealth = 1; }
        boostCoolDown = Random.Range(0.5f, 1.5f);
        backupAccel = accel;
        topSpeedBackup = topSpeed;
        ratings = 0;
        kills = 0;
        deaths = 0;
        currentLap = 0;

        Transform[] pathLine = AIPath.GetComponentsInChildren<Transform>();
        pathNodes = new List<Transform>();

        //count all the nodes so the ai knows where to go
        for (int i = 0; i < pathLine.Length; i++)
        {
            //not the parent object node though
            if (pathLine[i] != AIPath.transform)
            {
                pathNodes.Add(pathLine[i]);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Steering();
        Drive();
        CheckNodeDistance();
        //always round ratings to the nearest whole number
        Mathf.Round(ratings);

        xVelocity = AIRB.velocity.x;
        zVelocity = AIRB.velocity.z;
        if (transform.position.y >= yPositionCap)
        {
            transform.position = new Vector3(transform.position.x, yPositionCap, transform.position.z);
        }
        //make it so the vehicle cant spam boost collide with other
        if (collideBoostTimer > 0)
        {
            collideBoostTimer -= Time.deltaTime;
        }

        //sprite size increase
        if (AIPos.localScale.x <= initialSize)
        {
            AIPos.localScale = new Vector3(initialSize, initialSize, initialSize);
        }
        if (AIPos.localScale.x >= initialSize)
        {
            if (AIPos.position.y >= 2)
            {
                AIPos.localScale = new Vector3(AIPos.position.y / 1.3f, AIPos.position.y / 1.3f, initialSize);
            }
        }
        //limit max scale
        if (AIPos.localScale.x >= 5)
        {
            AIPos.localScale = new Vector3(5, 5, initialSize);
        }

        //death and respawning
        if (currentHealth <= 0 && death == false)
        {
            //stop moving, go invisible, spawn explosion prefab
            deaths += 1;
            death = true;
            respawnTimer = 3;
            AIRB.velocity = Vector3.zero;
            Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(90, 0, 0));
            //replace sprite renderer with mesh renderer in future
            AIRender.enabled = false;
            //if close enough to player, play explosion sound for this vehicle
            if (Vector3.Distance(transform.position, playerPos.position) <= 18f)
            {
                playerPos.gameObject.GetComponent<Player>().playerSound.PlayOneShot(playerPos.gameObject.GetComponent<Player>().explosion, 1f);
            }
        }
        if (death == true && respawnTimer >= 0)
        {
            respawnTimer -= Time.deltaTime;
            AIRB.velocity = Vector3.zero;
            AICol.enabled = false;
            AIWheel.enabled = false;
            AIWheel2.enabled = false;
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
            Debug.Log("Respawned AI");
            death = false;
            AICol.enabled = true;
            AIWheel.enabled = true;
            AIWheel2.enabled = true;
            AIRender.enabled = true;
            respawnInvuln = 3;
            currentHealth = maxHealth;
        }

        if (respawnInvuln >= 0)
        {
            respawnInvuln -= Time.deltaTime;
            //flash for invuln
            if (respawnInvuln >= 2 && respawnInvuln <= 2.5) { AIRender.enabled = false; }
            if (respawnInvuln <= 2 && respawnInvuln >= 1.5) { AIRender.enabled = true; }
            if (respawnInvuln <= 1.5 && respawnInvuln >= 1) { AIRender.enabled = false; }
            if (respawnInvuln <= 1 && respawnInvuln >= 0.5) { AIRender.enabled = true; }
            if (respawnInvuln <= 0.5 && respawnInvuln >= 0.2) { AIRender.enabled = false; }
            if (respawnInvuln <= 0.1) { AIRender.enabled = true; }
        }

        //speed caps
        if (AIRB.velocity.x >= topSpeed)
        {
            AIRB.velocity = new Vector3(topSpeed - Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z);
        }
        if (AIRB.velocity.z >= topSpeed)
        {
            AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y, topSpeed - Time.deltaTime);
        }
        if (AIRB.velocity.x <= -topSpeed)
        {
            AIRB.velocity = new Vector3(-topSpeed + Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z);
        }
        if (AIRB.velocity.z <= -topSpeed)
        {
            AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y, -topSpeed + Time.deltaTime);
        }


        //swerve into player
        if (Vector3.Distance(transform.position, playerPos.position) <= 8f && swerveTimer <= 0)
        {
            swerveTimer = 10;
            swerveAngle = Random.Range(-3f, 3f);

        }
        if (swerveTimer > 9.5f)
        {
            newSteerAngle = 0;
            angle += swerveAngle;
            maxSteerAngle = 0;
        }
        if (swerveTimer <= 9.5f)
        {
            maxSteerAngle = oldSteerAngle;
        }

        //cant boost if in front of player
        if (currentPathNode >= playerPos.GetComponent<Player>().currentPathNode || currentLap > playerPos.GetComponent<Player>().currentLap)
        {
            if (boosting == false)
            {
                boostCoolDown = 3;
            }
        }
        //boosting
        if (RaceHandler.raceStarted == true && gameObject.tag != "Vehicle Fodder")
        {
            boostCoolDown -= Time.deltaTime;
        }
        if (boostCoolDown <= 0 && boosting == false && gameObject.tag != "Vehicle Fodder")
        {
            boosting = true;
            accel *= 2f;
            topSpeed *= 3;
            Debug.Log("Ai Boost");
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            //if close enough to player, play boost sound for this vehicle
            if (Vector3.Distance(transform.position, playerPos.position) <= 18f)
            {
                playerPos.gameObject.GetComponent<Player>().playerSound.PlayOneShot(playerPos.gameObject.GetComponent<Player>().engineBoost, 1f);
            }
        }
        if (boostCoolDown <= -3 + Random.Range(-3,1) && boosting == true && gameObject.tag != "Vehicle Fodder")
        {
            boostCoolDown = Random.Range(5,15);
            boosting = false;
            
        }
        //reset speed values after boosting ends
        if (boosting == false && accel != backupAccel && gameObject.tag != "Vehicle Fodder")
        {
            accel = backupAccel;
            topSpeed = topSpeedBackup;
        }



        //placement tracking
        //compare lap and checkpoint with each opponent racer
        //if ahead of the racer, move up one place and move them down one place
        //only be able to change position while they havent completed the third lap
        if (currentLap <= 5 && currentPathNode != 0 && currentPathNode != pathNodes.Count)
        {
            //opponent1
            if ((currentLap > playerPos.GetComponent<Player>().currentLap) || currentLap == playerPos.GetComponent<Player>().currentLap && currentPathNode > playerPos.GetComponent<Player>().currentPathNode)
            {
                if (playerPos.GetComponent<Player>().placement <= placement && playerPos.GetComponent<Player>().currentPathNode != 0)
                {
                    placement -= 1;
                    playerPos.GetComponent<Player>().placement += 1;
                }
            }
            //opponent2
            if ((currentLap > opponent2.GetComponent<AIEngine>().currentLap) || currentLap == opponent2.GetComponent<AIEngine>().currentLap && currentPathNode > opponent2.GetComponent<AIEngine>().currentPathNode)
            {
                if (opponent2.GetComponent<AIEngine>().placement <= placement && opponent2.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent2.GetComponent<AIEngine>().placement += 1;
                }
            }
            //opponent3
            if ((currentLap > opponent3.GetComponent<AIEngine>().currentLap) || currentLap == opponent3.GetComponent<AIEngine>().currentLap && currentPathNode > opponent3.GetComponent<AIEngine>().currentPathNode )
            {
                if (opponent3.GetComponent<AIEngine>().placement <= placement && opponent3.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent3.GetComponent<AIEngine>().placement += 1;
                }
            }
            //opponent4
            if ((currentLap > opponent4.GetComponent<AIEngine>().currentLap) || currentLap == opponent4.GetComponent<AIEngine>().currentLap && currentPathNode > opponent4.GetComponent<AIEngine>().currentPathNode)
            {
                if (opponent4.GetComponent<AIEngine>().placement <= placement && opponent4.GetComponent<AIEngine>().currentPathNode != 0)
                {
                    placement -= 1;
                    opponent4.GetComponent<AIEngine>().placement += 1;
                }
            }
        }

    }

    private void CheckNodeDistance()
    {
        //check if close enough to node to confirm progress through track
        if(Vector3.Distance(transform.position,pathNodes[currentPathNode].position) <= 18f)
        {
            //confirm node is passed and set target to next node
            if (currentPathNode == pathNodes.Count - 1)
            {
                //new lap if at last node
                currentPathNode = 0;
                currentCheckPoint = 0;
                
            }
            else
            {
                //if crossing the first point, increase lap count by 1
                if (currentCheckPoint == 0)
                {
                    currentLap++;
                    //get points
                    ratings += 100;
                }
                //if not at end, go to next node
                currentPathNode++;
                currentCheckPoint++;

            }
        }
    }
    private void Drive()
    {
        //only drive if you havent finished race
        if (RaceHandler.raceStarted == true  && death == false /*&& currentLap < 4*/)
        {
            AIRB.AddForce(transform.up * accel);
            swerveTimer -= Time.deltaTime;
        }
        }

    //turning towards checkpoints
    private void Steering()
    {
        //find distance from ai to next node
        Vector3 relativeAngle = transform.InverseTransformPoint(pathNodes[currentPathNode].position);
        //convert relative distance to be a range of -1 to 1
       // relativeAngle /= relativeAngle.magnitude;

        newSteerAngle = (relativeAngle.x / relativeAngle.magnitude) * maxSteerAngle;
        transform.eulerAngles = new Vector3(90, transform.rotation.y, angle);
        AIWheel.steerAngle = newSteerAngle;
        AIWheel2.steerAngle = newSteerAngle;
        angle -= newSteerAngle;
      //  Debug.Log(relativeAngle);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BoostPad")
        {
            Debug.Log("BoostPad");
            AIRB.AddForce(transform.up * 500 * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            collideBoostTimer = 0.7f;

        }
        //pickups
        if (other.gameObject.tag == "HealthPickup" && gameObject.tag == "Vehicle")
        {
            if (other.GetComponent<Pickup>().pickupAble == true && currentHealth < maxHealth)
            {
                //restore hp
                currentHealth += maxHealth / 6;
                if (currentHealth >= maxHealth) { currentHealth = maxHealth; }
                other.GetComponent<Pickup>().respawnTimer = 4;
            }
        }
        if (other.gameObject.tag == "BoostPickup" && gameObject.tag == "Vehicle")
        {
            if (other.GetComponent<Pickup>().pickupAble == true)
            {
                if (boosting == true)
                {
                    //as ai's dont have fuel, just increase the length of their boost
                    boostCoolDown += 3;
                }
                other.GetComponent<Pickup>().respawnTimer = 4;
            }
        }

        if (other.gameObject.tag == "KillGrid" && fallingDeath == false)
        {
            //kill the ai instantly
            deaths += 1;
            ratings /= 2;
            currentHealth -= 1000;
            fallingDeath = true;
            boosting = false;
        }



    }

    void OnCollisionEnter(Collision other)
    {

        //colliding with other vehicle
        if ((other.gameObject.tag == "Vehicle" || other.gameObject.tag == "Vehicle Fodder" )&& collideBoostTimer <= 0)
        {
            other.gameObject.GetComponent<AIEngine>().AIRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f, 1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
            collideBoostTimer = 0.7f;
            Debug.Log("Collision with vehicle " + other.gameObject.name);
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            //lose health
            if (boosting == false && respawnInvuln <= 0 && RaceHandler.raceStarted == true) { currentHealth -= 4f; }
            //if death, give attacking vehicle points
            if (other.gameObject.tag == "Vehicle" && gameObject.tag == "Vehicle" && currentHealth <= 0)
            {
                //give opposing vehicle a quarter of their ratings and 1 kill
                other.gameObject.GetComponent<AIEngine>().ratings += ratings / 4;
                other.gameObject.GetComponent<AIEngine>().kills += 1;
                ratings /= 2;
            }
            //fodder enemy
            if (gameObject.tag == "Vehicle Fodder")
            {
                other.gameObject.GetComponent<AIEngine>().ratings += 75;
                other.gameObject.GetComponent<AIEngine>().kills += 1;
            }

        }
        if (other.gameObject.tag == "Player" && collideBoostTimer <= 0)
        {
            other.gameObject.GetComponent<Player>().playerRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f, 1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
            collideBoostTimer = 0.7f;
            Debug.Log("Collision with player");
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            if (boosting == false && respawnInvuln <= 0 && RaceHandler.raceStarted == true) { currentHealth -= 5f; }
            //if death, give attacking vehicle points
            if (currentHealth <= 0)
            {
                //give opposing vehicle a quarter of their ratings
                other.gameObject.GetComponent<Player>().ratings += ratings / 4;
                other.gameObject.GetComponent<Player>().kills += 1;
                ratings /= 2;
            }
            //fodder enemy
            if (gameObject.tag == "Vehicle Fodder") { other.gameObject.GetComponent<Player>().ratings += 75; }

        }
    }
}
