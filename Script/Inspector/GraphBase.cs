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
        [HideInInspector] public IRoot rootNode;

        public virtual Type GetRootNodeType() => typeof(Root<int>);

        public virtual void Initialize()
        {
            var type = GetRootNodeType();
            if (blackboard == null)
            {
                blackboard = CreateNode(typeof(Blackboard)) as Blackboard;
            }
            if (rootNode == null && ContainsNodeOfType(type) != null)
            {
                rootNode = ContainsNodeOfType(type) as IRoot;
            }
            else if (rootNode == null)
            {
                rootNode = CreateNode(GetRootNodeType()) as Root<int>;
            }
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }

        public virtual object Run(GraphVariables newstorage = null)
        {
            runtimeStorage = newstorage;
            return ((Root<int>)rootNode).GetValue(((Root<int>)rootNode).Ports.First());
        }

        private object CreateNode(Type type)
        {
            Node node = AddNode(type);

            if (node == null) return null;
            Undo.RegisterCreatedObjectUndo(node, "Create Node");
            node.position = new Vector2(500, 150);
            if (node.name == null || node.name.Trim() == "") node.name = NodeEditorUtilities.NodeDefaultName(type);
            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) AssetDatabase.AddObjectToAsset(node, this);

            return node;
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