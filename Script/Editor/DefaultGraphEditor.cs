using System;
using System.Reflection;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Graph
{
    [CustomNodeGraphEditor(typeof(DefaultGraph))]
    public class DefaultGraphEditor : NodeGraphEditor
    {
        public Node ContainsNodeOfType(Type type)
        {
            foreach (var node in ((DefaultGraph)target).nodes)
            {
                if (node.GetType() == type)
                {
                    return node;
                }
            }

            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            DefaultGraph graph = target as DefaultGraph;
            NodeEditorWindow.current.graphEditor = this;

            if (graph.blackboard == null)
            {
                var bb = CreateNode(typeof(Graph.Blackboard), new Vector2(0, 0));
                graph.blackboard = bb as Graph.Blackboard;
                graph.blackboard.InitializeBlackboard();
            }

            var graphRootNodeType = graph.GetRootNodeType();

            // we do not want to have two outputs
            if (graph.root == null && ContainsNodeOfType(graphRootNodeType) != null)
            {
                graph.root = (Root)ContainsNodeOfType(graphRootNodeType);
            }
            else if (graph.root == null)
            {
                var root = CreateNode(graphRootNodeType, new Vector2(500, 150));
                graph.root = root as Root;
            }
        }

        public override string GetPortTooltip(XNode.NodePort port)
        {
            return port.ValueType.ToString();
        }

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((Graph.DefaultGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            var typeToString = type.ToString();

            if (type.GetCustomAttribute<HideFromNodeMenu>(false) == null)
            {
                return base.GetNodeMenuName(type);
            }
            else
            {
                return null;
            }
        }

        public override void RemoveNode(Node node)
        {
            if (node != ((DefaultGraph)target).blackboard )
            {
                base.RemoveNode(node);
            }
        }
    }
}
