using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    // checks a boolean variable in the car object
    public class CheckerNode : ActionNode
    {
        public string inputString;

        private Data data;

        public override TreeResult Exec(Data data)
        {
            this.data = data;

            if (interpretInput(inputString))
                return TreeResult.Success;
            else
                return TreeResult.Failure;
        }

        private bool interpretInput(string input)
        {
            switch (input)
            {
                case "isFollower":
                    return data.driver.isFollower;
                case "lightDetected":
                    return data.driver.lightDetected;
                case "carBlocking":
                    return data.driver.carBlocking;
                case "isInching":
                    return data.driver.isInching;
                default:
                    Debug.Log("Invalid input for comparison: " + input);
                    return false;
            }
        }
    }
}

