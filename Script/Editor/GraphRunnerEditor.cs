using UnityEngine;
using UnityEditor;
using Graph;

namespace GraphEditor
{
    [CustomEditor(typeof(GraphRunner))]
    public class GraphRunnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GraphRunner runner = target as GraphRunner;

            HandleNewGraph(runner);
            HandleNewSo(runner);

            serializedObject.ApplyModifiedProperties();
        }

        void HandleNewSo(GraphRunner runner)
        {
            runner.So = (ScriptableObject)EditorGUILayout.ObjectField(runner.So, typeof(ScriptableObject), true);

            if (runner.So != null && runner.graph != null)
            {
                DrawSoValidity(runner);
            }
        }

        void    HandleNewGraph(GraphRunner runner)
        {
            var TmpGraph = runner.graph;
            TmpGraph = (Graph.TestGraph)EditorGUILayout.ObjectField(runner.graph, typeof(Graph.TestGraph), true);

            if (TmpGraph == null)
            {
                return;
            }
            else if (TmpGraph != runner.graph)
            {
                runner.graph = TmpGraph;
                runner.BuildValueDictionnary();
            }
        }

        void DrawSoValidity(GraphRunner runner)
        {
            var properties = runner.So.GetType().GetProperties();

            foreach (var property in runner.So.GetType().GetFields())
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(property.Name);

                if (runner.ContainsValue(property.Name, property.FieldType))
                {
                    EditorGUILayout.LabelField("    OK");
                }
                else
                {
                    EditorGUILayout.LabelField("    NOK");
                }

                GUILayout.EndHorizontal();
                //Debug.Log("Name: " + property.Name);// + " Value: " + property.GetValue(this));
            }
        }
    }
}