using UnityEngine;

namespace Trees
{
    public class SucceederNode : Base.TreeNode
    {
        [Input] public TreeKnobEmpty inputKnob;
        public SucceederNode() : base(TreeNodeType.Succeeder)
        { }

        public override TreeResult Exec(Data data)
        {
            return TreeResult.Success;
        }
    }
}
