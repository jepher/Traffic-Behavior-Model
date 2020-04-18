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
            Car follower = data.follower;
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
                    acceleration = (float)Math.Min(data.leader.acceleration + Math.Pow(follower.velocity - data.leader.velocity, 2) / (2 * (data.gap - data.follower.stop_gap)), follower.deceleration_max);
                    break;
                case 3:
                    acceleration = (float)Math.Min(Math.Pow(follower.velocity, 2) / (2 * (data.gap - data.follower.stop_gap)), follower.deceleration_max);
                    break;
                case 4:
                    acceleration = (float)(data.leader.acceleration - 0.25 * follower.deceleration_normal);
                    break;
                case 5:
                    acceleration = (float)Math.Min(Math.Max(Math.Pow(follower.velocity, 2) / (2 * data.light.gap), 0.5), follower.deceleration_max);
                    break;
                case 6:
                    acceleration = follower.deceleration_max;
                    break;
                case 7:
                    acceleration = follower.deceleration_normal;
                    break;
            }
            if (acceleration > 0)
                acceleration = -acceleration;

            data.follower.acceleration = acceleration;
            return TreeResult.Success;
        }
    }
}
