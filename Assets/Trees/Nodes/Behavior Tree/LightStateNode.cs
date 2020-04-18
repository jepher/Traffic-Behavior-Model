using UnityEngine;
using Trees.Base;

namespace Trees
{
    public class LightStateNode : DecoratorNode
    {
        public enum Color
        {
            Red = 0,
            Yellow,
            Green,
        }

        public Color color;

        public override bool Branch(Data data)
        {
            switch (color)
            {
                case Color.Red:
                    return data.light.currentState.Equals("RED");
                case Color.Yellow:
                    return data.light.currentState.Equals("YELLOW");
                case Color.Green:
                    return data.light.currentState.Equals("GREEN");
            }
            return false;
        }
    }
}

