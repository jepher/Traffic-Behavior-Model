using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Trees;
using Trees.Base;

namespace XNodeEditor
{
    [CustomNodeEditor(typeof(TreeNode))]
    public class TreeNodeEditor : NodeEditor
    {

        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            TreeNode node = target as TreeNode;
            TreeGraph graph = node.graph as TreeGraph;

            string title = target.name;
            GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            GUI.color = Color.white;
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            TreeNode node = target as TreeNode;
            TreeGraph graph = node.graph as TreeGraph;
        }
    }
}