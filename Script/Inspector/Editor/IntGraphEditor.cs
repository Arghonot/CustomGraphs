using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CustomGraph
{
    [CustomNodeGraphEditor(typeof(IntGraph))]
    public class LibnoiseGraphEditor : NodeGraphEditor
    {
        public override void RemoveNode(Node node)
        {
            if (node != ((IntGraph)target).blackboard &&
                !node.GetType().ToString().Contains("Root"))
            {
                base.RemoveNode(node);
            }
        }

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((IntGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (type.GetCustomAttribute<HideFromNodeMenu>(false) == null)
            {
                return base.GetNodeMenuName(type);
            }
            return string.Empty;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            IntGraph graph = target as IntGraph;
            NodeEditorWindow.current.graphEditor = this;

            if (graph.blackboard == null)
            {
                CreateNode(typeof(Blackboard), new Vector2(0, 0));
                graph.blackboard = (Blackboard)graph.nodes.Where(x => x.GetType() == typeof(Blackboard)).First();
            }
            if (graph.Root == null)
            {
                CreateNode(typeof(RootInt), new Vector2(0, 0));
                graph.Root = (RootInt)graph.nodes.Where(x => x.GetType() == typeof(RootInt)).First();
            }
        }
    }
}