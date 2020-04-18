using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    public class CompareNode : DecoratorNode
    {
        public enum Condition
        {
            Equal = 0,
            Less,
            LessEqual,
            Greater,
            GreaterEqual,
        }

        public Condition condition;
        public float coefficient1 = 1;
        public float coefficient2 = 1;
        public string inputString1;
        public string inputString2;

        private Data data;

        public override bool Branch(Data data)
        {
            this.data = data;
            float input1 = coefficient1 * interpretInput(inputString1);
            float input2 = coefficient2 * interpretInput(inputString2);

            switch (condition)
            {
                case Condition.Equal:
                    return Mathf.Approximately(input1, input2);
                case Condition.Less:
                    return input1 < input2;
                case Condition.LessEqual:
                    return input1 <= input2;
                case Condition.Greater:
                    return input1 > input2;
                case Condition.GreaterEqual:
                    return input1 >= input2;
            }
            return false;
        }

        private float interpretInput(string input)
        {
            switch (input)
            {
                case "gap":
                    return data.gap;
                case "gap_desired":
                    return data.follower.gap_desired;
                case "stop_gap":
                    return data.follower.stop_gap;
                case "gap_light_safe":
                    return data.follower.gap_light_safe;
                case "light_distance":
                    return data.light.gap;
                case "reaction_distance":
                    return data.follower.reaction_distance;
                case "speed_limit":
                    return data.speed_limit;
                case "v_l":
                    return data.leader.velocity;
                case "v_f":
                    return data.follower.velocity;
                case "v_desired":
                    return data.follower.gap_desired;
                case "v_inching":
                    return data.follower.velocity_inching;
                case "v_yellow":
                    return data.follower.velocity_yellow;
                case "a_l":
                    return data.leader.acceleration;
                case "deceleration_max":
                    return data.follower.deceleration_max;
                case "deceleration_light": // deceleration necessary to stop for light
                    return (float)Math.Pow(data.follower.velocity, 2) / (2 * data.light.gap);
                case "constant":
                    return 1;
                default:
                    Debug.Log("Invalid input for comparison");
                    return 0;
            }
        }
    }
}

