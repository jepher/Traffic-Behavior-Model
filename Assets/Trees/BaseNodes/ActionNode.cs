using UnityEngine;

namespace Trees.Base
{
    public class ActionNode : Base.TreeNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public TreeKnobEmpty inputKnob;

        public ActionNode() : base(TreeNodeType.Action)
        {}
    }
}
