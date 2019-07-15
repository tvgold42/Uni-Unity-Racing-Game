using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dampTime = 0.05f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Transform previewAI;
    public Camera gameCamera;
    public GameObject raceHandler;
    
    
    


    public static float shakeX;
    public static float shakeY;
    public static float shakeZ;

    public float cameraX;
    public float cameraZ;

    //shaking test
    public static Vector3 originPosition;
 
    public static float shake_decay;
    public static float shake_intensity;


    void Start()
    {
        gameCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {




        if (target && RaceHandler.racePreview == false)
        {
            Vector3 point = gameCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - gameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        //camera follow preview ai so the track is seen before the race starts
        if (previewAI && RaceHandler.racePreview == true)
        {
            Vector3 point = gameCamera.WorldToViewportPoint(previewAI.position);
            Vector3 delta = previewAI.position - gameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        if (shake_intensity > 0 && target.GetComponent<Player>().fuelBoosting == false)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity * 2;

            shake_intensity -= Time.deltaTime * 2f;
        }

        if (target.GetComponent<Player>().fuelBoosting == true)
        {
            transform.position += Random.insideUnitSphere * shake_intensity * 2;

            shake_intensity -= Time.deltaTime * 2f;
        }

        //lock y
        transform.position = new Vector3(transform.position.x, 67, transform.position.z);
        /*
        transform.position += new Vector3(shakeX, shakeY, shakeZ);

        if (shakeX >= 0) {shakeX -= Time.deltaTime;  }
        if (shakeX <= 0) { shakeX += Time.deltaTime; }
        if (Random.Range(0, 1) >= 0.5) { shakeX *= -1; }
        if (Random.Range(0, 1) >= 0.5) { shakeX *= -1; }

        if (shakeY >= 0) { shakeY -= Time.deltaTime;  }
        if (shakeY <= 0) { shakeY += Time.deltaTime; }
        if (Random.Range(0, 1) >= 0.5) { shakeY *= -1; }
        if (Random.Range(0, 1) >= 0.5) { shakeY *= -1; }

        if (shakeZ >= 0) { shakeZ -= Time.deltaTime; }
        if (shakeZ <= 0) { shakeZ += Time.deltaTime; }
        if (Random.Range(0, 1) >= 0.5) { shakeZ *= -1; }
        if (Random.Range(0, 1) >= 0.5) { shakeZ *= -1; }
        */


    }

    /*
     *    // public Transform playerPosition;
             cameraX = Player.playerX;
        cameraZ = Player.playerZ;
        float translation = Time.deltaTime * 100;
        gameObject.transform.position = new Vector3(cameraX , 60, cameraZ );

    void LateUpdate()
    {

    }
    */
}
