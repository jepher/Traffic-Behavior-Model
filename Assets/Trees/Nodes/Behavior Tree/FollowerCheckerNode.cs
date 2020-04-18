using UnityEngine;
using Trees.Base;

namespace Trees
{
    // checks if current car is a follower
    public class FollowerCheckerNode : DecoratorNode
    {
        public bool condition;
        public override bool Branch(Data data)
        {
            if (condition)
                return data.leader != null;
            else
                return data.leader == null;
        }
    }
}

