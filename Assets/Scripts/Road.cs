using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Road : MonoBehaviour
{
    public int speed_limit;
    private Renderer renderer;
    public Car mainCar; // car camera is centered on
    public GameObject Car;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    public int maxCars;
    private Spawner[] spawners;

    void Start()
    {
        minSpawnDelay = 5;
        maxSpawnDelay = 10;
        maxCars = 10;

        // initialize spawners
        spawners = new Spawner[2];
        GameObject parent = transform.parent.Find("Cars").gameObject;
        spawners[0] = new Spawner(Car, minSpawnDelay, maxSpawnDelay, maxCars, transform, 0, mainCar, parent);
        spawners[1] = new Spawner(Car, minSpawnDelay, maxSpawnDelay, maxCars, transform, 1, mainCar, parent);
    }

    void Update()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.Update();
        }
    }
}
