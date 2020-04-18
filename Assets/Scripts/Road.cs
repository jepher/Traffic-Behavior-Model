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

    private Vector3 spawnPoint1; // left to right
    private Vector3 spawnPoint2; // right to left
    private Vector3 destination1;
    private Vector3 destination2;
    private Spawner[] spawners;

    // Start is called before the first frame update
    void Start()
    {
        minSpawnDelay = 5;
        maxSpawnDelay = 10;
        maxCars = 10;

        // get renderer
        renderer = gameObject.GetComponent<Renderer>();

        // calculate spawn points
        spawnPoint1 = transform.position + new Vector3(-renderer.bounds.size.x / 2, 0, -renderer.bounds.size.z / 4);
        spawnPoint2 = transform.position + new Vector3(renderer.bounds.size.x / 2, 0, renderer.bounds.size.z / 4);

        // calculate destinations
        float padding = 10; 
        destination1 = transform.position + new Vector3(renderer.bounds.size.x / 2 + padding, 0, -renderer.bounds.size.z / 4);
        destination2 = transform.position + new Vector3(-(renderer.bounds.size.x / 2 + padding), 0, renderer.bounds.size.z / 4);

        // initialize spawners
        spawners = new Spawner[2];
        GameObject parent = transform.parent.GetComponent<Intersection>().cars;
        spawners[0] = new Spawner(Car, gameObject, minSpawnDelay, maxSpawnDelay, maxCars, spawnPoint1, destination1, 1, mainCar, parent);
        spawners[1] = new Spawner(Car, gameObject, minSpawnDelay, maxSpawnDelay, maxCars, spawnPoint2, destination2, -1, mainCar, parent);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.Update();
        }
    }
}
