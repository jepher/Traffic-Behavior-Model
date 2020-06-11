using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
        private GameObject Car;
        private Transform road;
        private Vector3 spawnPoint;
        private Vector3 destination;
        private int direction; // 0: left to right, 1: right to left
        private Car mainCar;
        private GameObject parent;
        private GameObject[] cars;
        private int numCars;
        private int maxCars;
        private float elapsedTime;
        private float minSpawnDelay;
        private float maxSpawnDelay;
        private float spawnDelay;

        public Spawner(GameObject Car, float minSpawnDelay, float maxSpawnDelay, int maxCars, Transform road, int direction, Car mainCar, GameObject parent)
        {
            this.Car = Car;
            this.direction = direction;
            Vector3 roadDimensions = road.gameObject.GetComponent<Renderer>().bounds.size;
            if (direction == 0) // left to right
            {
                spawnPoint = road.position + new Vector3(-roadDimensions.x / 2, 0, -roadDimensions.z / 4);
                destination = road.position + new Vector3(roadDimensions.x / 2, 0, -roadDimensions.z / 4);
            }
            else // right to left
            {
                spawnPoint = road.position + new Vector3(roadDimensions.x / 2, 0, roadDimensions.z / 4);
                destination = road.position + new Vector3(-roadDimensions.x / 2, 0, roadDimensions.z / 4);
            }
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
            // adding cars
            if (numCars < maxCars && Math.Abs(spawnPoint.z - mainCar.transform.position.z) <= 700)
            {
                if (elapsedTime == 0)
                    spawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);

                elapsedTime += Time.deltaTime;
                if (elapsedTime >= spawnDelay)
                {
                    elapsedTime = 0;
                    GameObject carGameObject = Instantiate(Car, spawnPoint, Quaternion.AngleAxis(90 + 180 * direction, Vector3.up));
                    carGameObject.transform.parent = parent.transform;
                    Car car = carGameObject.GetComponent<Car>();
                    car.destination = destination;
                    cars[numCars] = carGameObject;
                    numCars++;
                }
            }
            else
                elapsedTime = 0;

            // removing cars
            if (numCars > 0 && cars[0].GetComponent<Car>().reachedDestination)
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