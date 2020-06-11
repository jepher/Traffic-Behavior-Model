using UnityEngine;
using Trees.Base;

namespace Trees
{
    public class LightStateNode : ActionNode
    {        public enum Color
        {
            Red = 0,
            Yellow,
            Green,
        }

        public Color color;

        public override TreeResult Exec(Data data)
        {
            bool comparison = false;
            switch (color)
            {
                case Color.Red:
                    comparison = data.light.light.currentState.Equals("RED");
                    break;
                case Color.Yellow:
                    comparison = data.light.light.currentState.Equals("YELLOW");
                    break;
                case Color.Green:
                    comparison = data.light.light.currentState.Equals("GREEN");
                    break;
            }

            if (comparison)
                return TreeResult.Success;
            else
                return TreeResult.Failure;
        }
    }
}

