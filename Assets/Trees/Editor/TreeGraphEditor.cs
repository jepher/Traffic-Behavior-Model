using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;
using XNodeEditor;

namespace XNodeEditor
{
    [CustomNodeGraphEditor(typeof(TreeGraph))]
    public class TreeGraphEditor : NodeGraphEditor
    {

        /// <summary> 
        /// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
        /// </summary>
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.Namespace == "Trees")
            {
                return base.GetNodeMenuName(type).Replace("Trees/", "");
            }
            else return null;
        }
    }
}