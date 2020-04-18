using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public float timer = 60;
    public GameObject Road;
    public GameObject cars;
    public GameObject[] lightsObject1 = new GameObject[2];
    public GameObject[] lightsObject2 = new GameObject[2];
    TrafficLight[] lights1;
    TrafficLight[] lights2;
    int lights1State = 1; // which set of lights go green first
    int lights2State = 0;
    // Use this for initialization
    void Start()
    {
        lights1 = new TrafficLight[2];
        lights2 = new TrafficLight[2];
        for (int i = 0; i < lightsObject1.Length; i++)
        {
            lights1[i] = lightsObject1[i].GetComponent<TrafficLight>();
            lights2[i] = lightsObject2[i].GetComponent<TrafficLight>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        TrafficState(lights1State, timer, lights1);
        TrafficState(lights2State, timer, lights2);
        if (timer <= 0)
            timer = 60;
    }
    public void TrafficState(int state, float timer, TrafficLight[] lights)
    {
        switch (state)
        {
            case 1: //RGY
                if (timer <= 60 && timer > 30)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("RED");
                    }
                }
                else if (timer <= 30 && timer > 10)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("GREEN");
                    }
                }
                else if (timer <= 10 && timer > 0)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("YELLOW");
                    }
                }
                break;
            case 0: //GYR
                if (timer <= 60 && timer > 40)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("GREEN");
                    }
                }
                else if (timer <= 40 && timer > 30)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("YELLOW");
                    }
                }
                else if (timer <= 30 && timer > 0)
                {
                    foreach (TrafficLight light in lights)
                    {
                        light.ChangeColor("RED");
                    }
                }
                break;
        }
    }
}