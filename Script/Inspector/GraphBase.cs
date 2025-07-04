using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CustomGraph
{
    public class GraphBase : NodeGraph
    {
        [HideInInspector] public Blackboard blackboard;
        public GraphVariables originalStorage = new GraphVariables();
        [HideInInspector] public GraphVariables runtimeStorage;
        [HideInInspector] public bool CanRun;
        [HideInInspector] public Root root;
        public virtual Type GetRootNodeType() => typeof(Root);

        public virtual void Initialize()
        {
            var type = GetRootNodeType();
            if (blackboard == null)
            {
                var newBlackboard = AddNode<Blackboard>();
                blackboard = newBlackboard;
            }
            if (root == null && ContainsNodeOfType(type) != null)
            {
                root = (Root)ContainsNodeOfType(type);
            }
            else if (root == null)
            {
                XNode.Node node = AddNode(type);
                if (node == null) return;
                Undo.RegisterCreatedObjectUndo(node, "Create Node");
                node.position = new Vector2(500, 150);
                if (node.name == null || node.name.Trim() == "") node.name = NodeEditorUtilities.NodeDefaultName(type);
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) AssetDatabase.AddObjectToAsset(node, this);
                root = node as Root;
            }
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }
        public virtual object Run(GraphVariables newstorage = null)
        {
            runtimeStorage = newstorage;
            return root.GetValue(root.Ports.First());
        }

        public Node ContainsNodeOfType(Type type)
        {
            foreach (var node in nodes)
            {
                if (node.GetType() == type) return node;
            }
            return null;
        }
    }
}