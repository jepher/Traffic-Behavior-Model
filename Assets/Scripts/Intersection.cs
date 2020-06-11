using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public float timer;
    public GameObject[] lightsObject1;
    public GameObject[] lightsObject2;
    private TrafficLight[] lights1;
    private TrafficLight[] lights2;
    private int lights1State; // which set of lights go green first
    private int lights2State;
    void Start()
    {
        timer = 60;
        lights1State = 1;
        lights2State = 0;
        lightsObject1 = new GameObject[2];
        lightsObject1[0] = transform.GetChild(0).gameObject;
        lightsObject1[1] = transform.GetChild(1).gameObject;
        lightsObject2 = new GameObject[2];
        lightsObject2[0] = transform.GetChild(2).gameObject;
        lightsObject2[1] = transform.GetChild(3).gameObject;
        lights1 = new TrafficLight[2];
        lights2 = new TrafficLight[2];
        for (int i = 0; i < lightsObject1.Length; i++)
        {
            lights1[i] = lightsObject1[i].GetComponent<TrafficLight>();
            lights2[i] = lightsObject2[i].GetComponent<TrafficLight>();
        }
    }
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