using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Graph
{
    [CustomNodeGraphEditor(typeof(IntGraph))]
    public class LibnoiseGraphEditor : XNodeEditor.NodeGraphEditor
    {
        List<Type> HiddenTypes = new List<Type>()
        {
            typeof(RootInt),
            typeof(Graph.Leaf<int>),
            typeof(Graph.Branch<int>),
            typeof(Graph.Blackboard),
            typeof(Graph.NodeBase)
        };

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
            NodeEditorWindow.current.titleContent =
                new GUIContent(((IntGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (!HiddenTypes.Contains(type) && !type.ToString().Contains("Root"))
            {
                return base.GetNodeMenuName(type);
            }

            else return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            IntGraph graph = target as IntGraph;
            NodeEditorWindow.current.graphEditor = this;

            if (graph.blackboard == null)
            {
                CreateNode(typeof(Blackboard), new Vector2(0, 0));
                graph.blackboard = (Blackboard)graph.nodes.
                    Where(x => x.GetType() == typeof(Blackboard)).
                    First();
            }

            if (graph.Root == null)
            {
                CreateNode(typeof(RootInt), new Vector2(0, 0));
                graph.Root = (RootInt)graph.nodes.
                    Where(x => x.GetType() == typeof(RootInt)).
                    First();
            }
        }
    }
}