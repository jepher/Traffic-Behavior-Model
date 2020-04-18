using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
        private GameObject Car;
        private GameObject road;
        public Vector3 spawnPoint;
        public Vector3 destination;
        private int direction;
        private Car mainCar;
        private GameObject parent;
        private GameObject[] cars;
        private int numCars;
        private int maxCars;
        private float elapsedTime;
        private float minSpawnDelay;
        private float maxSpawnDelay;
        private float spawnDelay;

        public Spawner(GameObject Car, GameObject road, float minSpawnDelay, float maxSpawnDelay, int maxCars, Vector3 spawnPoint, Vector3 destination, int direction, Car mainCar, GameObject parent)
        {
            this.Car = Car;
            this.road = road;
            this.spawnPoint = spawnPoint;
            this.destination = destination;
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
            // adding cars
            if (numCars < maxCars && Math.Abs(spawnPoint.z - mainCar.transform.position.z) <= 700)
            {
                if (elapsedTime == 0)
                    spawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);

                elapsedTime += Time.deltaTime;
                if (elapsedTime >= spawnDelay)
                {
                    elapsedTime = 0;
                    GameObject carGameObject = Instantiate(Car, spawnPoint, Quaternion.AngleAxis(90 * direction, Vector3.up));
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