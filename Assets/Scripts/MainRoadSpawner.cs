using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MainRoadSpawner : MonoBehaviour
{
        private GameObject Car;
        private Transform road;
        private int direction; // 0: bottom to top, 1: top to bottom
        private Car mainCar;
        private GameObject parent;
        private GameObject[] cars;
        private int numCars;
        private int maxCars;
        private float elapsedTime;
        private float minSpawnDelay;
        private float maxSpawnDelay;
        private float spawnDelay;

        public MainRoadSpawner(GameObject Car, float minSpawnDelay, float maxSpawnDelay, int maxCars, Transform road, int direction, Car mainCar, GameObject parent)
        {
            this.Car = Car;
            this.road = road;
            this.direction = direction;
            this.minSpawnDelay = minSpawnDelay;
            this.maxSpawnDelay = maxSpawnDelay;
            this.maxCars = maxCars;
            this.mainCar = mainCar;
            this.parent = parent;
            cars = new GameObject[maxCars];
            numCars = 0;
            elapsedTime = 0;
            spawnDelay = 0;
        }

        public void Update()
        {
            Vector3 roadDimensions = road.gameObject.GetComponent<Renderer>().bounds.size;
            float spawnDistance = 200; // distance to spawn from main car
            Vector3 spawnPoint;
            if(direction == 0)
                spawnPoint = mainCar.transform.position + new Vector3(0, 0, spawnDistance);
            else
                spawnPoint = mainCar.transform.position + new Vector3(-roadDimensions.x / 2, 0, spawnDistance);

            // adding cars
            if (numCars < maxCars)
            {
                if (elapsedTime == 0)
                    spawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);

                elapsedTime += Time.deltaTime;
                if (elapsedTime >= spawnDelay)
                {
                    elapsedTime = 0;
                    GameObject carGameObject = Instantiate(Car, spawnPoint, Quaternion.AngleAxis(180 * direction, Vector3.up));
                    carGameObject.transform.parent = parent.transform;
                    Car car = carGameObject.GetComponent<Car>();
                    if (direction == 0)
                        car.destination = road.position + new Vector3(roadDimensions.x / 2, 0, -roadDimensions.z / 4);
                    cars[numCars] = carGameObject;
                    numCars++;
                }
            }
            else
                elapsedTime = 0;

            // removing cars
            if (numCars > 0 )
            {
                Car firstCar = cars[0].GetComponent<Car>();
                    if (direction == 0 && firstCar.reachedDestination
                    || direction == 1 && Math.Abs(firstCar.transform.position.z - mainCar.transform.position.z) > 200)
                {
                    Destroy(cars[0]);
                    cars[0] = null;
                    for (int i = 1; i < numCars; i++)
                        cars[i - 1] = cars[i];
                    numCars--;
                    cars[numCars] = null;
                }
            }
        }
}