using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(BT.TestGraph))]
public class TestGraphEditor : XNodeEditor.NodeGraphEditor
{
    public override Texture2D GetGridTexture()
    {
        NodeEditorWindow.current.titleContent = new GUIContent(((BT.TestGraph)target).name);

        return base.GetGridTexture();
    }

    public override void OnGUI()
    {
        // Keep repainting the GUI of the active NodeEditorWindow
        NodeEditorWindow.current.Repaint();
    }
}
