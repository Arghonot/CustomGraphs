using System;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(BT.TestGraph))]
public class TestGraphEditor : XNodeEditor.NodeGraphEditor
{
    public override Texture2D GetGridTexture()
    {
        NodeEditorWindow.current.titleContent = new GUIContent(((BT.TestGraph)target).name);

        return base.GetGridTexture();
    }

    public override string GetNodeMenuName(Type type)
    {
        if (type != typeof(BT.Blackboard))
        {
            return base.GetNodeMenuName(type);
        }

        else return null;
    }

    public override void RemoveNode(Node node)
    {
        if (node != ((BT.TestGraph)target).blackboard)
        {
            base.RemoveNode(node);
        }
    }

    public override void OnGUI()
    {
        NodeEditorWindow.current.Repaint();
    }
}
