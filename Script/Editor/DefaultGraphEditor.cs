using System;
using System.Reflection;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CustomGraph
{
    [CustomNodeGraphEditor(typeof(GraphBase))]
    public class DefaultGraphEditor : NodeGraphEditor
    {
        public Node ContainsNodeOfType(Type type)
        {
            foreach (var node in ((GraphBase)target).nodes)
            {
                if (node.GetType() == type)
                {
                    return node;
                }
            }
            return null;
        }

        public override string GetPortTooltip(XNode.NodePort port) => port.ValueType.ToString();

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((CustomGraph.GraphBase)target).name);

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
            if (node != ((GraphBase)target).blackboard )
            {
                base.RemoveNode(node);
            }
        }
    }
}
