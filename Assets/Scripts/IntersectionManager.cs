using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    public int[] positions;
    public GameObject Intersection;
    public Car mainCar; // car camera is centered on
    public GameObject Car;
    public float minSpawnDelay = 5;
    public float maxSpawnDelay = 10;
    public int maxCars = 20;
    private int position;
    private Spawner[] spawners;
    private Renderer renderer;

    private Vector3 start1;
    private Vector3 start2;
    private Vector3 destination1;
    private Vector3 destination2;


    // Start is called before the first frame update
    void Start()
    {
        minSpawnDelay = 3;
        maxSpawnDelay = 6;
        positions = new int[]{ 145, 300, 500, 750, 1000, 1400, 1650};
        foreach(int position in positions)
        {
            GameObject intersection = Instantiate(Intersection, new Vector3(0, 0, position), Quaternion.identity);
            intersection.transform.parent = gameObject.transform;

            GameObject roadGameObject = intersection.GetComponent<Intersection>().Road;
            Road road = roadGameObject.GetComponent<Road>();
            road.mainCar = mainCar;
        }

        position = 0;
        renderer = gameObject.GetComponent<Renderer>();
        start1 = transform.position + new Vector3(renderer.bounds.size.x / 4, 0, -renderer.bounds.size.z / 2);
        start2 = transform.position + new Vector3(-renderer.bounds.size.x / 4, 0, renderer.bounds.size.z / 2);
        destination1 = transform.position + new Vector3(renderer.bounds.size.x / 4, 0, renderer.bounds.size.z / 2);
        destination2 = transform.position + new Vector3(-renderer.bounds.size.x / 4, 0, -renderer.bounds.size.z / 2);

        spawners = new Spawner[2];
        GameObject parent = GameObject.Find("/Cars");
        spawners[0] = new Spawner(Car, gameObject, minSpawnDelay, maxSpawnDelay, maxCars, new Vector3(renderer.bounds.size.x / 4, 0, positions[position + 1] + 10), destination1, 0, mainCar, parent); // bottom to top
        spawners[1] = new Spawner(Car, gameObject, minSpawnDelay, maxSpawnDelay, maxCars, new Vector3(-renderer.bounds.size.x / 4, 0, positions[position + 1] - 10), start1, 2, mainCar, parent); // top to bottom
    }

    void Update()
    {
        if(position < positions.Length && mainCar.transform.position.z >= positions[position])
        {
            position++;
            if (position == positions.Length - 1)
            {
                spawners[0].spawnPoint = destination1;
                spawners[1].spawnPoint = start2;
            }
            else
            {
                spawners[0].spawnPoint = new Vector3(renderer.bounds.size.x / 4, 0, positions[position + 1] + 10);
                spawners[1].spawnPoint = new Vector3(-renderer.bounds.size.x / 4, 0, positions[position + 1] - 10);
            }
            spawners[1].destination = new Vector3(-renderer.bounds.size.x / 4, 0, positions[position - 1] + 10);
        }
        foreach (Spawner spawner in spawners)
        {
            spawner.Update();
        }
    }
}
