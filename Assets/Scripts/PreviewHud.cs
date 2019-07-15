using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHud : MonoBehaviour
{
    void Update()
    {
        //just destory any objects with this script once the preview of the track has ended
        if (RaceHandler.racePreview == false)
        {
            Destroy(gameObject);
        }
    }
}
