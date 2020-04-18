using UnityEngine;
using XNode;

namespace Trees
{
    [CreateAssetMenu(fileName = "NewTreeGraph", menuName = "TreeGraph")]
    public class TreeGraph : NodeGraph
    {
        public StartNode GetStartNode()
        {
            StartNode startNode = null;
            foreach (var node in nodes)
            {
                startNode = node as StartNode;
                if (startNode != null)
                {
                    return startNode;
                }
            }
            return null;
        }

        public void Exec(Data data)
        {
            var startNode = GetStartNode();
            if (startNode != null)
            {
                startNode.Exec(data);
            }
        }
    }
}
