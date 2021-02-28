using System;
using System.Collections.Generic;
//using System.ge
using UnityEngine;
using XNode;
using XNodeEditor;

namespace GraphEditor
{
    [CustomNodeGraphEditor(typeof(Graph.DefaultGraph))]
    public class DefaultGraphEditor : XNodeEditor.NodeGraphEditor
    {
        List<Type> HiddenTypes = new List<Type>()
        {
            typeof(Graph.Blackboard),
            typeof(Graph.RootInt),
            typeof(Graph.Single)
        };

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((Graph.DefaultGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (!HiddenTypes.Contains(type) &&
                !type.ToString().Contains("Root") &&
                !type.ToString().Contains("Leaf"))
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
            if (node != ((Graph.DefaultGraph)target).blackboard)
            {
                base.RemoveNode(node);
            }
        }

        public override void OnGUI()
        {
            NodeEditorWindow.current.Repaint();
        }
    }
}
