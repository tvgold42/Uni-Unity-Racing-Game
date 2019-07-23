using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float baseScale;
    public float respawnTimer;
    public bool pickupAble;
    public SpriteRenderer pickupRender;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale.x;
        pickupAble = true;
        pickupRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
            pickupAble = false;
            pickupRender.color = new Color(1f, 1f, 1f, 0.3f);

        }
        if (respawnTimer <= 0 && pickupAble == false)
        {
            pickupAble = true;
            pickupRender.color = new Color(1f, 1f, 1f, 1);
        }
    }
}
