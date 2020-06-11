using UnityEngine;
using Trees;
using System;
using System.Collections;

public class TrafficLightObserver
{
    public TrafficLight light;
    public float gap;
    public float timeObserved;

    public TrafficLightObserver(TrafficLight light)
    {
        this.light = light;
        gap = 0;
        timeObserved = 0;
    }

    public string getCurrentState()
    {
        return light.currentState;
    }
}

public class Data
{
    public Car leader;
    public Car driver;
    public float gap;
    public TrafficLightObserver light;
    public float speed_limit;

    public Data(Car leader, Car driver)
    {
        this.leader = leader;
        this.driver = driver;
        if (leader != null)
            gap = Vector3.Magnitude(leader.transform.position - driver.transform.position) - driver.transform.localScale.z;
        else
            gap = 0;
        light = null;
        speed_limit = 30;
    }
}

public class Car : MonoBehaviour
{
    public bool logging;
    public int id;

    public float velocity;
    public float velocity_max;
    public float velocity_desired;
    public float velocity_safe;
    public float velocity_inching; // velocity to close gap with a stopped leader
    public float velocity_yellow; // yellow light cruise velocity
    public float acceleration;
    public float acceleration_max;
    public float deceleration_normal;
    public float deceleration_max;
    public float t_p;
    public float T;
    public float reaction_delay;
    public float b; // strength of noise
    public float gap_desired;
    public float stop_gap; // preferred minimum distance from other cars
    public float intersection_width;
    public float reaction_distance;
    public float stoppable_distance; // minimum distance required to stop

    [SerializeField]
    public TreeGraph behaviorTree;
    public bool isFollower;
    public bool lightDetected; 
    public bool carBlocking; // check if there is a car travelling the opposite axis blocking the road
    public bool isInching; // check if car is moving forward to close gap with stationary object
    private Data data;

    public Vector3 destination;
    public bool reachedDestination;

    public void Start()
    {
        logging = false;
        id = 0;

        velocity = 25;
        velocity_max = 35;
        velocity_inching = 2;
        velocity_yellow = 10;
        acceleration_max = 3;
        deceleration_normal = 3.05f;
        deceleration_max = 6.04f;
        t_p = 1.2f;
        T = 3;
        reaction_delay = 1;
        b = 0.2f;
        stoppable_distance = 0;
        stop_gap = 5;
        intersection_width = 20;
        reaction_distance = 100;

        isFollower = false;
        lightDetected = false;
        carBlocking = false;
        isInching = false;

        reachedDestination = false;
        Console.Clear();
    }

    private void FixedUpdate()
    {
        // check if reached destination
        if (destination != null && (transform.position == destination || Vector3.Dot(transform.position, transform.forward) > Vector3.Dot(destination, transform.forward)))
            reachedDestination = true;

        // create data object
        data = new Data(null, this);

        // update reaction distance
        reaction_distance = Math.Max(100, (float)(0.5 * velocity * ((velocity / deceleration_normal) + reaction_delay)));

        // update stoppable distance
        stoppable_distance = (float)Math.Pow(velocity, 2) / (2 * deceleration_max);

        // reset booleans
        lightDetected = false;
        carBlocking = false;

        // reset acceleration
        acceleration = 0;

        // intersection perception
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, reaction_distance);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject detectedObject = hitColliders[i].gameObject;
            Vector3 forward = transform.forward;
            Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
            Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;

            Vector3 hitPosition = hitColliders[i].transform.position;
            Vector3 hitVector = hitPosition - transform.position;
            Vector3 hitForward = hitColliders[i].transform.forward;

            float gap = Math.Abs(Vector3.Dot(hitVector, forward)) - (transform.localScale.z / 2); // subtract by transform.localScale.z / 2 to account for front half of car, since the position of gameObjects are centered
            if (Vector3.Dot(hitVector, forward) > 0) {// only want objects in front of car
                // traffic light
                if (hitColliders[i].tag == "Light"
                    && hitForward == -forward // traffic light facing opposite direction as front of car
                    && gap > intersection_width) // car has not passed stopping line for intersection
                {
                    TrafficLight detectedLight = detectedObject.GetComponent<TrafficLight>();

                    data.light = new TrafficLightObserver(detectedLight);

                    data.light.gap = gap - intersection_width;

                    lightDetected = true;
                }
                // check for cars running red light
                else if (hitColliders[i].tag == "Car"
                    && Vector3.Dot(hitForward, forward) < 1E-5 // detected car travelling in opposite axis as car
                    && gap < 20) {
                    Car detectedCar = detectedObject.GetComponent<Car>();
                    if ((hitForward == right // car going right
                         && Vector3.Dot(hitVector, left) >= 0 // left side of current car
                         && detectedCar.stoppable_distance >= Vector3.Dot(hitVector, left) - transform.localScale.z / 2 - transform.localScale.x / 2) // car cannot stop before closing axial distance
                        ||
                        (hitForward == left // car going left
                         && Vector3.Dot(hitVector, right) >= 0 // right side of current car
                         && detectedCar.stoppable_distance >= Vector3.Dot(hitVector, right) - transform.localScale.z / 2 - transform.localScale.x / 2))
                    {
                        carBlocking = true;
                    }
                }
            }
        }

        RaycastHit carHit;
        // car sensor
        if (Physics.Raycast(transform.position, transform.forward, out carHit, reaction_distance) // object detected
            && carHit.collider.tag == "Car" // object is car
            && carHit.collider != transform.GetComponent<Collider>() // ignore own hitbox
            && carHit.collider.transform.forward == transform.forward) // object detected facing same direction as front of car
        {
            Car leader = carHit.collider.gameObject.GetComponent<Car>();
            float gap = Vector3.Magnitude(leader.transform.position - transform.position) - transform.localScale.z;
            
            if (data.light != null
                && data.light.getCurrentState() == "RED"
                && (data.light.gap < gap)) // check if red light separates follower and leader
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                isFollower = false;
                data.leader = null;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                isFollower = true;
                data.leader = leader;
                data.gap = gap;
            }
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            data.leader = null;
            isFollower = false;
        }

        // follower behavior
        if (isFollower)
        {
            // calculate desired gap
            gap_desired = Math.Max(stop_gap, velocity * t_p);

            // calculate safe velocity
            velocity_safe = data.leader.velocity + (data.gap - data.leader.velocity * reaction_delay) / (reaction_delay + (data.leader.velocity + data.driver.velocity) / (2 * data.driver.deceleration_max));

            // calculate desired velocity
            velocity_desired = Math.Min(data.driver.velocity_max, Math.Min(data.driver.velocity + data.driver.acceleration, velocity_safe));
        }

        // run behavior tree
        if (behaviorTree != null && data != null)
        {
            behaviorTree.Exec(data);
        }

        if (logging)
        {
            //Debug.Log("leader id: " + data.leader.id);
            //Debug.Log("velocity: " + velocity + ", leader velocity: " + data.leader.velocity + ", velocity desired: " + velocity_desired + ", velocity safe: " + velocity_safe);
            //Debug.Log("acceleration: " + acceleration);
            Debug.Log("gap: " + data.gap + ", desired gap: " + gap_desired + ", stoppable distance: " + stoppable_distance);
            //if (data.light != null)
            //{
            //    Debug.Log("light distance: " + data.light.gap);
            //}
            //Debug.Log("detection distance: " + 0.5 * velocity * ((velocity / -acceleration) + reaction_delay));
            //Debug.Log("delta time: " + Time.fixedDeltaTime + ", velocity change: " + acceleration * Time.fixedDeltaTime);
            //Debug.Log("reaction distance: " + reaction_distance);
            //Debug.Break();
        }

        // update velocity
        velocity += acceleration * Time.fixedDeltaTime;

        // prevent going backwards
        if (velocity <= 0)
        {
            acceleration = 0;
            velocity = 0;
        }

        // update position
        Vector3 movement = velocity * transform.forward;
        transform.position += movement * Time.fixedDeltaTime;
    }
}

