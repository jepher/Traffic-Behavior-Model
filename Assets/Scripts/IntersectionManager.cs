using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    public int[] positions;
    public GameObject Intersection;
    public Car mainCar; // car camera is centered on
    public GameObject Car;
    private int position;
    private MainRoadSpawner[] spawners;
    private Renderer renderer;

    private Vector3 start1;
    private Vector3 start2;
    private Vector3 destination1;
    private Vector3 destination2;
    void Start()
    {
        positions = new int[]{ 100, 300, 500, 750, 1000, 1400, 1650};
        // create intersections
        foreach(int position in positions)
        {
            GameObject intersection = Instantiate(Intersection, new Vector3(0, 0, position), Quaternion.identity);
            intersection.transform.parent = gameObject.transform;

            Transform roadGameObject = intersection.transform.Find("Road");
            Road road = roadGameObject.gameObject.GetComponent<Road>();
            road.mainCar = mainCar;
        }

        // create spawners
        float minSpawnDelay = 3;
        float maxSpawnDelay = 6;
        spawners = new MainRoadSpawner[2];
        GameObject parent = GameObject.Find("/Cars");
        spawners[0] = new MainRoadSpawner(Car, minSpawnDelay, maxSpawnDelay, 100, transform, 0, mainCar, parent); // bottom to top
        spawners[1] = new MainRoadSpawner(Car, minSpawnDelay, maxSpawnDelay, 6, transform, 1, mainCar, parent); // top to bottom
    }

    void Update()
    {
        //foreach (MainRoadSpawner spawner in spawners)
        //{
        //    spawner.Update();
        //}
        spawners[0].Update();
    }
}
