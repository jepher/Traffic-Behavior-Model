namespace Trees
{
    public class StartNode : Base.TreeNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override)] public TreeKnobEmpty outputKnob;

        public StartNode() : base(TreeNodeType.Start)
        {}

        public override TreeResult Exec(Data data)
        {
            var next = GetNext();
            if (next != null) {
                return next.Exec(data);
            }
            return TreeResult.Failure;
        }
    }
}
