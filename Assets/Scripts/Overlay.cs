using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public float selfTime;

    void Update()
    {

        selfTime += Time.deltaTime;

        if (selfTime >= 2)
        {
            Destroy(gameObject);
        }
    }
}
