using UnityEngine;
using UnityEditor;
using CustomGraph;

namespace GraphEditor
{
    [CustomEditor(typeof(GraphRunner<DefaultGraph>))]
    public class GraphRunnerEditor : Editor
    {
        GraphRunner<DefaultGraph> runner;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            runner = target as GraphRunner<DefaultGraph>;

            if (target == null) return;

            HandleNewGraph();
            HandleNewSo();

            serializedObject.ApplyModifiedProperties();
        }

        void HandleNewSo()
        {
            runner.So = (ScriptableObject)EditorGUILayout.ObjectField(runner.So, typeof(ScriptableObject), true);
        }

        void    HandleNewGraph()
        {
            var TmpGraph = runner.graph;
            TmpGraph = (CustomGraph.DefaultGraph)EditorGUILayout.ObjectField(runner.graph, typeof(CustomGraph.DefaultGraph), true);

            if (TmpGraph == null)
            {
                return;
            }
            else if (TmpGraph != runner.graph)
            {
                runner.graph = TmpGraph;
            }
        }
    }
}