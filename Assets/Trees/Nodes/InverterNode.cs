using UnityEngine;

namespace Trees
{
    public class InverterNode : Base.TreeNode
    {
        [Input] public TreeKnobEmpty inputKnob;
        [Output] public TreeKnobEmpty outputKnob;
        public InverterNode() : base(TreeNodeType.Inverter)
        { }

        public override TreeResult Exec(Data data)
        {
            var next = GetNext();
            if (next != null)
            {
                if (next.Exec(data) == TreeResult.Success)
                    return TreeResult.Failure;
                else
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
