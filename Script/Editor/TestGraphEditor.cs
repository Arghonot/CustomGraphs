using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[CustomNodeGraphEditor(typeof(BT.TestGraph))]
public class TestGraphEditor : XNodeEditor.NodeGraphEditor
{
    public override void OnGUI()
    {
        // Keep repainting the GUI of the active NodeEditorWindow
        NodeEditorWindow.current.Repaint();
    }
}
