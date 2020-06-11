using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    public class CompareNode : ActionNode
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

        public override TreeResult Exec(Data data)
        {
            this.data = data;
            float input1 = coefficient1 * interpretInput(inputString1);
            float input2 = coefficient2 * interpretInput(inputString2);
            Boolean comparison = false ;

            switch (condition)
            {
                case Condition.Equal:
                    comparison = Mathf.Approximately(input1, input2);
                    break;
                case Condition.Less:
                    comparison = input1 < input2;
                    break;
                case Condition.LessEqual:
                    comparison = input1 <= input2;
                    break;
                case Condition.Greater:
                    comparison = input1 > input2;
                    break;
                case Condition.GreaterEqual:
                    comparison = input1 >= input2;
                    break;
            }

            if (comparison)
                return TreeResult.Success;
            else
                return TreeResult.Failure;
        }

        private float interpretInput(string input)
        {
            switch (input)
            {
                case "gap":
                    return data.gap;
                case "gap desired":
                    return data.driver.gap_desired;
                case "stop gap":
                    return data.driver.stop_gap;
                case "stoppable distance":
                    return data.driver.stoppable_distance;
                case "light distance":
                    return data.light.gap;
                case "reaction distance":
                    return data.driver.reaction_distance;
                case "speed limit":
                    return data.speed_limit;
                case "v_l":
                    return data.leader.velocity;
                case "v":
                    return data.driver.velocity;
                case "v_desired":
                    return data.driver.velocity_desired;
                case "v_inching":
                    return data.driver.velocity_inching;
                case "v_yellow":
                    return data.driver.velocity_yellow;
                case "a_l":
                    return data.leader.acceleration;
                case "max deceleration":
                    return data.driver.deceleration_max;
                case "light deceleration": // deceleration necessary to stop for light
                    return (float)Math.Pow(data.driver.velocity, 2) / (2 * data.light.gap);
                case "constant":
                    return 1;
                default:
                    Debug.Log("Invalid input for comparison: " + input);
                    return 0;
            }
        }
    }
}

