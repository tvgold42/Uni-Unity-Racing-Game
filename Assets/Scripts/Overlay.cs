using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public float selfTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // transform.eulerAngles = new Vector3(180, 0, 0);

        selfTime += Time.deltaTime;

        if (selfTime >= 2)
        {
            Destroy(gameObject);
        }
    }
}
