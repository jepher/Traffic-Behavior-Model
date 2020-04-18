using UnityEngine;
using Trees.Base;

namespace Trees
{
    // check if behavior of current car is affected by a light
    public class LightCheckerNode : DecoratorNode
    {
        public bool condition;
        public override bool Branch(Data data)
        {
            if (condition)
                // has light in front that is not green
                return data.light != null;
            else
                // no light in front that is not green
                return data.light == null;
        }
    }
}

