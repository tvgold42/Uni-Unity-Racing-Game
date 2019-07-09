using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine : MonoBehaviour
{

    public Transform AIPath;
    public Transform AIPos;
    public Rigidbody AIRB;
    public WheelCollider AIWheel;
    public WheelCollider AIWheel2;
    public float maxSteerAngle = 40f;
    public float topSpeed = 500;
    public float accel;
    public float newSteerAngle;
    public float angle;
    public float initialSize;

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
            }
            else
            {
                //if not at end, go to next node
                currentPathNode++;
            }
        }
    }
    private void Drive()
    {
        AIRB.AddForce(transform.up *  accel);
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
            AIRB.AddForce(transform.up * topSpeed * 20f);
            Instantiate(boostEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);

        }


    }
}
