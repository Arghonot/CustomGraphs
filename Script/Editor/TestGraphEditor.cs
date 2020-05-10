using System;
using System.Collections.Generic;
//using System.ge
using UnityEngine;
using XNode;
using XNodeEditor;

namespace GraphEditor
{
    [CustomNodeGraphEditor(typeof(Graph.TestGraph))]
    public class TestGraphEditor : XNodeEditor.NodeGraphEditor
    {
        List<Type> HiddenTypes = new List<Type>()
        {
            typeof(Graph.Blackboard),
            typeof(Graph.RootInt)
        };

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((Graph.TestGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (!HiddenTypes.Contains(type) && !type.ToString().Contains("Root"))
            {
                Debug.Log(type.ToString());
                return base.GetNodeMenuName(type);
            }

            else return null;
        }

        public override void RemoveNode(Node node)
        {
            if (node != ((Graph.TestGraph)target).blackboard)
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
