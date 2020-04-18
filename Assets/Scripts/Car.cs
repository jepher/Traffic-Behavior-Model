using UnityEngine;
using Trees;
using System;

public class Data
{
    public Car leader;
    public Car follower;
    public float gap;
    public TrafficLight light;
    public float speed_limit;

    public Data(Car leader, Car follower)
    {
        this.leader = leader;
        this.follower = follower;
        if (leader != null)
            gap = Vector3.Magnitude(leader.transform.position - follower.transform.position) - follower.transform.localScale.z;
        else
            gap = 0;
        light = null;
        speed_limit = 30;
    }
}

public class Car : MonoBehaviour
{
    public bool isControllable;
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
    public float gap_light_safe; // distance where driver can safely stop for traffic light
    public float stop_gap;
    public float intersection_width;
    public float reaction_distance;

    [SerializeField]
    public TreeGraph decisionTree;
    public TreeGraph behaviorTree;
    private bool isFollower;
    public bool carBlocking; // check if there is a car travelling the opposite axis blocking the road
    private Data data;

    public Vector3 destination;
    public bool reachedDestination;

    public Car()
    {
        isControllable = false;
        logging = false;
        id = 0;

        velocity = 25;
        velocity_max = 35;
        velocity_inching = 5;
        velocity_yellow = 10; 
        acceleration_max = 3;
        deceleration_normal = 3.05f;
        deceleration_max = 6.04f;
        t_p = 1.2f;
        T = 3;
        reaction_delay = 1;
        b = 0.2f; 
        gap_light_safe = 20; 
        stop_gap = 5;
        intersection_width = 20;
        reaction_distance = 100;

        isFollower = false;
        carBlocking = false;
        
        reachedDestination = false;
    }

    public void Start()
    {
        Console.Clear();
    }

    private void FixedUpdate()
    {
        // check if reached destination
        if (destination != null && (transform.position == destination || Vector3.Dot(transform.position, transform.forward) > Vector3.Dot(destination, transform.forward)))
            reachedDestination = true;

        // create data object
        data = new Data(null, this);

        // update following distance
        reaction_distance = Math.Max(100, (float)(0.5 * velocity * ((velocity / deceleration_normal) + reaction_delay)));

        // update traffic light stopping distance
        gap_light_safe = (float)Math.Pow(velocity, 2) / (2 * deceleration_max);

        // reset carBlocking
        carBlocking = false;

        // reset acceleration
        acceleration = 0;

        // intersection perception
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, reaction_distance);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Vector3 forward = transform.forward;
            Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
            Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
            Vector3 currentAxis = Vector3.Scale(forward, forward);
            Vector3 oppositeAxis = Vector3.Scale(left, left);

            Vector3 hitPosition = hitColliders[i].transform.position;
            Vector3 hitVector = hitPosition - transform.position;
            Vector3 hitForward = hitColliders[i].transform.forward;

            float gap = Vector3.Magnitude(Vector3.Scale(hitVector, currentAxis) - (transform.localScale.z / 2) * forward);
            if (Vector3.Dot(hitVector, forward) > 0) {// object in front of car
                // traffic light
                if(hitColliders[i].tag == "Light" 
                    && hitForward == -forward // traffic light facing opposite direction as front of car
                    && gap > intersection_width) // car has not passed stopping line 
                {
                    data.light = hitColliders[i].gameObject.GetComponent<TrafficLight>();
                    data.light.gap = gap - intersection_width;
                }
                // car at intersection
                else if (hitColliders[i].tag == "Car" 
                    && (Math.Abs(Vector3.Dot(hitForward, oppositeAxis)) == 1) // detected car travelling in opposite axis as car
                    && (gap < 20
                        && ((Vector3.Magnitude(Vector3.Scale(hitVector, oppositeAxis) - (transform.localScale.z / 2) * left) < 15 
                            && hitForward == right 
                            && Vector3.Dot(hitVector, left) <= 0) 
                            || (Vector3.Magnitude(Vector3.Scale(hitVector, oppositeAxis) - (transform.localScale.z / 2) * right) < 5 
                            && hitForward == left
                            && Vector3.Dot(hitVector, right) >= 0)))) // detected car is in the middle of the intersection
                {
                    carBlocking = true;
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
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            isFollower = true;
            Car leader = carHit.collider.gameObject.GetComponent<Car>();
            float gap = Vector3.Magnitude(leader.transform.position - transform.position) - transform.localScale.z;
            
            if (data.light != null
                && data.light.currentState == "RED"
                && (data.light.gap < gap)) // check if red light separates follower and leader
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                data.leader = null;
                isFollower = false;
            }
            else
            {
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


        // run behavior tree
        if (behaviorTree != null && data != null)
        {
            behaviorTree.Exec(data);
        }
        float temp = acceleration; // used to determine max acceleration from behavior or decision tree

        // follower behavior
        if (isFollower)
        {
            // calculate desired gap

            gap_desired = Math.Max(stop_gap, velocity * t_p);

            // calculate safe velocity
            velocity_safe = data.leader.velocity + (data.gap - data.leader.velocity * reaction_delay) / (reaction_delay + (data.leader.velocity + data.follower.velocity) / (2 * data.follower.deceleration_max));

            // calculate desired velocity
            velocity_desired = Math.Min(data.follower.velocity_max, Math.Min(data.follower.velocity + data.follower.acceleration, velocity_safe));

            if (logging)
            {
                //Debug.Log("leader id: " + data.leader.id);
                //Debug.Log("velocity: " + velocity + ", leader velocity: " + data.leader.velocity + ", velocity desired: " + velocity_desired);
                //Debug.Log("acceleration: " + acceleration);
                //Debug.Log("gap: " + data.gap + ", desired gap: " + gap_desired + ", stop gap: " + stop_gap);
                if (data.light != null)
                {
                    //Debug.Log("light distance: " + data.light.gap);
                }
                //Debug.Log("detection distance: " + 0.5 * velocity * ((velocity / -acceleration) + reaction_delay));
                //Debug.Log("delta time: " + Time.fixedDeltaTime + ", velocity change: " + acceleration * Time.fixedDeltaTime);
                //Debug.Log("reaction distance: " + reaction_distance);
                //Debug.Break();
            }

            // run decision tree
            if (decisionTree != null && data != null)
            {
                decisionTree.Exec(data);
            }
        }

        // take keyboard input
        else
        {
            if (isControllable)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (velocity <= 12.19)
                        acceleration = 1.1f;
                    else
                        acceleration = 0.37f;
                    velocity += acceleration * Time.fixedDeltaTime;
                }
                else if (Input.GetKey(KeyCode.DownArrow)) {
                    velocity -= deceleration_normal * Time.fixedDeltaTime;
                }
            }
        }

        // update velocity
        if (temp < 0 || acceleration < 0)
            acceleration = Math.Min(temp, acceleration); // determine max deceleration between behavior and decision tree
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

