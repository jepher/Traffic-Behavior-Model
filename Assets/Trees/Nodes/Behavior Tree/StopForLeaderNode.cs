using UnityEngine;
using Trees.Base;

namespace Trees
{
    public class StopForLeaderNode : ActionNode
    {
        public override TreeResult Exec(Data data)
        {
            if (data.gap - data.driver.stop_gap <= data.driver.stoppable_distance)
                return TreeResult.Success;
            else
                return TreeResult.Failure;
        }
    }
}

