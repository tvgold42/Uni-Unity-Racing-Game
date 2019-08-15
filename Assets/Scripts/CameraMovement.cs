using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dampTime = 0.05f;
    private Vector3 velocity = Vector3.zero;
    public Transform targetPlayer;
    public Transform previewAI;
    public Camera gameCamera;
    public GameObject raceHandler;
    
    public static float shakeX;
    public static float shakeY;
    public static float shakeZ;

    public float cameraX;
    public float cameraZ;

    //shaking
    public static Vector3 originPosition;
    public static float shake_decay;
    public static float shake_intensity;


    void Start()
    {
        gameCamera = GetComponent<Camera>();
    }


    void Update()
    {
        //track the player
        if (targetPlayer && RaceHandler.racePreview == false)
        {
            Vector3 point = gameCamera.WorldToViewportPoint(targetPlayer.position);
            Vector3 delta = targetPlayer.position - gameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        //camera follow preview ai so the track is seen before the race starts
        if (previewAI && RaceHandler.racePreview == true)
        {
            Vector3 point = gameCamera.WorldToViewportPoint(previewAI.position);
            Vector3 delta = previewAI.position - gameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        //shaking
        if (shake_intensity > 0.5f && targetPlayer.GetComponent<Player>().fuelBoosting == false && raceHandler.GetComponent<RaceHandler>().paused == false)
        {
            
            transform.position = originPosition;
            transform.position += Random.insideUnitSphere * shake_intensity * 2;
            shake_intensity -= Time.deltaTime * 2f;
        }

        if (targetPlayer.GetComponent<Player>().fuelBoosting == true && raceHandler.GetComponent<RaceHandler>().paused == false)
        {
            transform.position += Random.insideUnitSphere * shake_intensity * 2;

            shake_intensity -= Time.deltaTime * 2f;
        }

        //lock y
        transform.position = new Vector3(transform.position.x, 80, transform.position.z);


    }
}
