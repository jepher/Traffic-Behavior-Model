using UnityEngine;
using Trees.Base;

namespace Trees
{
    public class ToggleInchingNode : ActionNode
    {
        public bool input;
        public override TreeResult Exec(Data data)
        {
            data.driver.isInching = input;
            return TreeResult.Success;
        }
    }
}
