using System.Collections.Generic;
using UnityEngine;

namespace Trees
{
    public class SequencerNode : Base.TreeNode
    {
        [Input] TreeKnobEmpty inputKnob;
        [Output] TreeKnobEmpty outpuKnob;

        protected List<Base.TreeNode> TreeNodeList;

        public SequencerNode() : base(TreeNodeType.Sequencer)
        {
            TreeNodeList = new List<Base.TreeNode>();
        }

        protected override void Init()
        {
            base.Init();

            TreeNodeList.Clear();
            Base.TreeNode TreeNode = null;
            foreach (var port in Outputs) {
                if (port.IsConnected) {
                    int cnt = port.ConnectionCount;
                    for (int i = 0; i < cnt; ++i) {
                        var p = port.GetConnection(i);
                        if (p != null) {
                            TreeNode = p.node as Base.TreeNode;
                            if (TreeNode != null) {
                                TreeNodeList.Add(TreeNode);
                            }
                        }
                    }
                }
            }
        }

        public override TreeResult Exec(Data data)
        {
            foreach (var node in TreeNodeList) {
                if (node.Exec(data) == TreeResult.Failure) {
                    return TreeResult.Failure;
                }
            }
            return TreeResult.Success;
        }
    }
}
