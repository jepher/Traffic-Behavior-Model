using UnityEngine;
using Trees.Base;

namespace Trees
{
    public class AccelerateNode : ActionNode
    {
        public int id;
        public override TreeResult Exec(Data data)
        {
            if (data.follower.logging)
            {
                Debug.Log("Car ID: " + data.follower.id + ", ACCELERATE");
                //Debug.Break();
            }
            float acceleration = 0;

            switch (id)
            {
                case 0:
                    if (data.follower.velocity <= 12.19)
                        acceleration = 1.1f;
                    else
                        acceleration = 0.37f;
                    break;
                case 1:
                    acceleration = data.follower.acceleration_max;
                    break;
            }
            
            data.follower.acceleration = acceleration;
            return TreeResult.Success;
        }
    }
}
