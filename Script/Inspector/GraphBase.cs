using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    public class GraphBase : NodeGraph, ISerializationCallbackReceiver
    {
        [HideInInspector] public Blackboard blackboard;
        public GraphVariables originalStorage = new GraphVariables();
        [HideInInspector] public GraphVariables runtimeStorage;
        [HideInInspector] public bool CanRun;
        [HideInInspector] public RootBase rootNode;

        public virtual Type GetRootNodeType() => typeof(RootBase);

        public virtual void Initialize()
        {
            var type = GetRootNodeType();
            if (blackboard == null)
            {
                blackboard = CreateNode(typeof(Blackboard)) as Blackboard;
            }
            if (rootNode == null && ContainsNodeOfType(type) != null)
            {
                rootNode = ContainsNodeOfType(type) as RootBase;
            }
            else if (rootNode == null)
            {
                rootNode = CreateNode(GetRootNodeType()) as RootBase;
            }
        }

        public virtual object Run(GraphVariables newstorage = null)
        {
            if (newstorage != null) runtimeStorage = newstorage;
            else
                runtimeStorage = originalStorage.CreateDeepCopy();
            return rootNode.GetValue((rootNode).Ports.First());
        }

        private object CreateNode(Type type)
        {
            Node node = AddNode(type);

            if (node == null) return null;
//#if UNITY_EDITOR
//            Undo.RegisterCreatedObjectUndo(node, "Create Node");
//            if (node.name == null || node.name.Trim() == "") node.name = NodeEditorUtilities.NodeDefaultName(type);
//            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) AssetDatabase.AddObjectToAsset(node, this);
//#endif
            node.position = new Vector2(500, 150);

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

        public virtual void OnAfterDeserialize() { }

        public virtual void OnBeforeSerialize()
        {
            if (blackboard != null && originalStorage != null)
            {
                blackboard.storage = originalStorage;
            }
        }
    }
}