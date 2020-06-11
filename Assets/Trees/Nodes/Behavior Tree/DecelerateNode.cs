using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    public class DecelerateNode : ActionNode
    {
        public int id;
        public override TreeResult Exec(Data data)
        {
            Car follower = data.driver;
            if (follower.logging){
                Debug.Log("Car ID: " + follower.id + ", DECELERATE: " + id);
            }

            float acceleration = 0;
            switch (id)
            {
                case 0:
                    acceleration = Math.Min((follower.velocity - follower.velocity_desired) / follower.T, follower.deceleration_normal);
                    break;
                case 1:
                    acceleration = (float)(Math.Pow(follower.velocity, 2) - Math.Pow(data.leader.velocity, 2)) / (2 * data.gap);
                    break;
                case 2:
                    acceleration = (float)(Math.Max(-data.leader.acceleration, 0) + Math.Pow(follower.velocity - data.leader.velocity, 2) / (2 * (data.gap - data.driver.stop_gap)));
                    break;
                case 3:
                    acceleration = (float)(Math.Max(-data.leader.acceleration, 0) - 0.25 * follower.deceleration_normal);
                    break;
                case 4:
                    acceleration = (float)Math.Max(Math.Pow(follower.velocity, 2) / (2 * data.light.gap), 0.5);
                    break;
                case 5:
                    acceleration = follower.deceleration_max;
                    break;
                case 6:
                    acceleration = follower.deceleration_normal;
                    break;
            }

            if (Math.Abs(acceleration) > follower.deceleration_max) // deceleration cannot exceed physical limitations
            {
                acceleration = follower.deceleration_max;
            }
            else if(Math.Abs(acceleration) < 0.5) // prevent infinite deceleration (deceleration approaches but does not reach 0)
            {
                acceleration = 0.5f;
            }

            if (acceleration > 0) // ensure car is decelerating
                acceleration = -acceleration;

            if(acceleration != acceleration) // NaN: divide by zero error when gap is 0 
            {
                acceleration = 0;
            }
            data.driver.acceleration = acceleration;
            return TreeResult.Success;
        }
    }
}
