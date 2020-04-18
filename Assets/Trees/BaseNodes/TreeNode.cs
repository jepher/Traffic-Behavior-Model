using System;
using XNode;

namespace Trees.Base
{
    public class TreeNode : Node
    {
        public TreeNodeType NodeType { get; protected set; }

        public TreeNode(TreeNodeType nodeType)
        {
            NodeType = nodeType;
        }

        public virtual TreeResult Exec(Data data)
        {
            return TreeResult.Success;
        }

        public virtual TreeNode GetNext()
        {
            var port = GetOutputPort("outputKnob");
            TreeNode next = null;
            if (port != null && port.IsConnected) {
                next = port.Connection.node as TreeNode;
            }
            return next;
        }

        public virtual void Setup()
        { }
    }
}