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
    public float maxSteerAngle = 40f;
    public float topSpeed = 500;
    public float accel;
    public float newSteerAngle;
    private float oldSteerAngle;
    public float angle;
    public float initialSize;
    public float collideBoostTimer;
    public float xVelocity;
    public float zVelocity;

    public float yPositionCap;

    public float swerveTimer = 8;
    public float swereAngle;

    public float currentLap;
    public float currentCheckPoint;

    public GameObject boostEffect;
    public GameObject landEffect;

    private List<Transform> pathNodes;

    private int currentPathNode = 0;


    // Start is called before the first frame update
    void Start()
    {
        AIPos = GetComponent<Transform>();
        AIRB = GetComponent<Rigidbody>();
        initialSize = transform.localScale.x;
        currentLap = 0;
        oldSteerAngle = maxSteerAngle;

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

        //speedcaps
        /*
        if (AIRB.velocity.x >= topSpeed)
        {
            AIRB.velocity = new Vector3(topSpeed - Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z);
            if (AIRB.velocity.x >= 60) { AIRB.velocity = new Vector3(60 - Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z); }
        }
        if (AIRB.velocity.z >= topSpeed)
        {
            AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y, topSpeed - Time.deltaTime);
            if (AIRB.velocity.z >= 60) { AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y, 60 - Time.deltaTime); }
        }
        if (AIRB.velocity.x <= -topSpeed)
        {
            AIRB.velocity = new Vector3(-topSpeed + Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z);
            if (AIRB.velocity.x <= -60) { AIRB.velocity = new Vector3(-60 + Time.deltaTime, AIRB.velocity.y, AIRB.velocity.z); }
        }
        if (AIRB.velocity.z <= -topSpeed)
        {
            AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y, -topSpeed + Time.deltaTime);
            if (AIRB.velocity.z <= -60) { AIRB.velocity = new Vector3(AIRB.velocity.x, AIRB.velocity.y,  - 60 + Time.deltaTime); }
        }
        */
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
            swereAngle = Random.Range(-3f, 3f);

        }
        if (swerveTimer > 9.5f)
        {
            newSteerAngle = 0;
            angle += swereAngle;
            maxSteerAngle = 0;
        }
        if (swerveTimer <= 9.5f)
        {
            maxSteerAngle = oldSteerAngle;
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
        if (RaceHandler.raceStarted == true /*&& currentLap < 4*/)
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


    }

    void OnCollisionEnter(Collision other)
    {

        //colliding with other vehicle
        if (other.gameObject.tag == "Vehicle" && collideBoostTimer <= 0)
        {
            other.gameObject.GetComponent<AIEngine>().AIRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f, 1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
            //poomf effect
            collideBoostTimer = 0.7f;
            Debug.Log("Collision with vehicle " + other.gameObject.name);
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

        }
        if (other.gameObject.tag == "Player" && collideBoostTimer <= 0)
        {
            other.gameObject.GetComponent<Player>().playerRB.velocity += new Vector3(xVelocity * 1.25f * Random.Range(0.85f, 1.25f), 0, zVelocity * 1.25f * Random.Range(0.85f, 1.25f));
            //poomf effect
            collideBoostTimer = 0.7f;
            Debug.Log("Collision with player");
            Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

        }
    }
}
