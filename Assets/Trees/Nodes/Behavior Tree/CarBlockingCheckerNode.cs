using UnityEngine;
using Trees.Base;

namespace Trees
{
    // checks if a car is blocking the road
    public class CarBlockingCheckerNode : DecoratorNode
    {
        public bool condition;
        public override bool Branch(Data data)
        {
            if (condition)
                return data.follower.carBlocking;
            else
                return !data.follower.carBlocking;
        }
    }
}

