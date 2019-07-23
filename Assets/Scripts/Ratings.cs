using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratings : MonoBehaviour
{
    //the racer to track the ratings of
    public GameObject racer;
    public SpriteRenderer ratingSprite;
    //the various sprites to indicate ratings, worst to best
    public Sprite ratings1;
    public Sprite ratings2;
    public Sprite ratings3;
    public Sprite ratings4;
    public Sprite ratings5;


    // Start is called before the first frame update
    void Start()
    {
        ratingSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (racer.gameObject.tag == "Player")
        {
            if (racer.GetComponent<Player>().ratings <= 200){ratingSprite.sprite = ratings1; }
            if (racer.GetComponent<Player>().ratings >= 200 && racer.GetComponent<Player>().ratings < 800) { ratingSprite.sprite = ratings2; }
            if (racer.GetComponent<Player>().ratings >= 800 && racer.GetComponent<Player>().ratings < 1600) { ratingSprite.sprite = ratings3; }
            if (racer.GetComponent<Player>().ratings >= 1600 && racer.GetComponent<Player>().ratings < 3200) { ratingSprite.sprite = ratings4; }
            if (racer.GetComponent<Player>().ratings >= 3200 && racer.GetComponent<Player>().ratings < 6400) { ratingSprite.sprite = ratings5; }
        }
        if (racer.gameObject.tag == "Vehicle")
        {
            if (racer.GetComponent<AIEngine>().ratings <= 200) { ratingSprite.sprite = ratings1; }
            if (racer.GetComponent<AIEngine>().ratings >= 200 && racer.GetComponent<AIEngine>().ratings < 800) { ratingSprite.sprite = ratings2; }
            if (racer.GetComponent<AIEngine>().ratings >= 800 && racer.GetComponent<AIEngine>().ratings < 1600) { ratingSprite.sprite = ratings3; }
            if (racer.GetComponent<AIEngine>().ratings >= 1600 && racer.GetComponent<AIEngine>().ratings < 3200) { ratingSprite.sprite = ratings4; }
            if (racer.GetComponent<AIEngine>().ratings >= 3200 && racer.GetComponent<AIEngine>().ratings < 6400) { ratingSprite.sprite = ratings5; }
        }
    }
}
