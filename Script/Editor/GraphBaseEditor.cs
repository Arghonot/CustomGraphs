using UnityEditor;
using XNodeEditor;

namespace CustomGraph
{
    [CustomNodeGraphEditor(typeof(GraphBase))]
    public class GraphBaseEditor : NodeGraphEditor
    {
        public override void OnCreate()
        {
            base.OnCreate();
            GraphBase graph = target as GraphBase;
            NodeEditorWindow.current.graphEditor = this;
            graph.Initialize();

            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }
    }
}