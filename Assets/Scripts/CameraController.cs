using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float altitude_normal;
    public float altitude_max;
    public float altitude;
    public GameObject follow;
    public float zoomSpeed;

    void Start()
    {
        altitude_normal = 100;
        altitude_max = 200;
        altitude = altitude_normal;
        zoomSpeed = 10;
    }
    void Update()
    {
        if (follow.GetComponent<Car>().velocity == 0) // zoom out if car stopped
        {
            if (altitude < altitude_max)
                altitude += zoomSpeed * Time.deltaTime;
        }
        else { 
            if (altitude > altitude_normal)
                altitude -= zoomSpeed * Time.deltaTime;
        }
        transform.position = follow.transform.position + new Vector3(0, altitude, 0);

    }
}
