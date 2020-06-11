using UnityEngine;

namespace Trees
{
    public class DebugNode : Base.TreeNode
    {
        [Input] public TreeKnobEmpty inputKnob;
        [Output] public TreeKnobEmpty outputKnob;

        public string message;

        public DebugNode() : base(TreeNodeType.Debug)
        {}

        public override TreeResult Exec(Data data)
        {
            if (data.driver.logging)
            {
                Debug.Log(message);
                Debug.Break();
            }
            var next = GetNext();
            if (next != null)
            {
                return next.Exec(data);
            }
            return TreeResult.Success;
        }
    }
}
