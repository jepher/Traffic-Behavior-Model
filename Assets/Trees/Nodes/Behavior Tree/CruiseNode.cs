using UnityEngine;
using Trees.Base;
using System;

namespace Trees
{
    public class CruiseNode: ActionNode
    {
        public override TreeResult Exec(Data data)
        {
            if (data.driver.logging)
            {
                Debug.Log("Car ID: " + data.driver.id + ", CRUISE");
                //Debug.Break();
            }

            // calculate velocity with randomization
            data.driver.velocity = Math.Max(0, data.driver.velocity - data.driver.b * UnityEngine.Random.Range(-1.0f, 1.0f));
            // reset acceleration
            data.driver.acceleration = 0;

            return TreeResult.Success;
        }
    }
}
