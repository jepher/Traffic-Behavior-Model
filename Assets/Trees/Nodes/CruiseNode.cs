using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    public class CruiseNode: ActionNode
    {
        public override TreeResult Exec(Data data)
        {
            if (data.follower.logging)
            {
                Debug.Log("Car ID: " + data.follower.id + ", CRUISE");
                //Debug.Break();
            }

            // calculate velocity with randomization
            data.follower.velocity = Math.Max(0, data.follower.velocity - data.follower.b * UnityEngine.Random.Range(-1.0f, 1.0f));
            // reset acceleration
            data.follower.acceleration = 0;

            return TreeResult.Success;
        }
    }
}
