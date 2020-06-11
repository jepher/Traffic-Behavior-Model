using System.Collections.Generic;
using UnityEngine;

namespace Trees
{
    public class SelectorNode : Base.TreeNode
    {
        [Input] public TreeKnobEmpty inputKnob;
        [Output(dynamicPortList = true)] public TreeKnobEmpty[] outputKnob;

        protected List<Base.TreeNode> TreeNodeList;

        public SelectorNode() : base(TreeNodeType.Selector)
        {
            TreeNodeList = new List<Base.TreeNode>();
        }

        protected override void Init()
        {
            base.Init();
            TreeNodeList.Clear();
            Base.TreeNode node = null;
            for (int i = 0; i < outputKnob.Length; i++){
                var port = GetPort("outputKnob " + i);
                if (port.IsConnected) {
                    var p = port.Connection;
                    if (p != null) {
                        node = p.node as Base.TreeNode;
                        if (node != null) {
                            TreeNodeList.Add(node);
                        }
                    }
                }
            }
        }
        

        public override TreeResult Exec(Data data)
        {
            foreach (var node in TreeNodeList) {
                if (node.Exec(data) == TreeResult.Success) {
                    return TreeResult.Success;
                }
            }
            return TreeResult.Failure;
        }
    }
}
