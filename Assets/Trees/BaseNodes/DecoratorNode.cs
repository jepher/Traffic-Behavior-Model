using UnityEngine;

namespace Trees.Base
{
    public class DecoratorNode : Base.TreeNode
    {
        [Input] public TreeKnobEmpty inputKnob;
        [Output] public TreeKnobEmpty outputKnob;

        public DecoratorNode() : base(TreeNodeType.Decorator)
        { }

        public override TreeResult Exec(Data data)
        {
            if (Branch(data)) {
                Base.TreeNode TreeNode = GetNext();
                if (TreeNode != null) {
                    return TreeNode.Exec(data);
                }
                return TreeResult.Success;
            }
            return TreeResult.Failure;
        }

        public virtual bool Branch(Data data)
        {
            return true;
        }
    }
}
