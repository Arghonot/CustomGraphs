using UnityEngine;
using UnityEditor;
using CustomGraph;
using System;

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

            //if (runner.So != null && runner.graph != null)
            //{
            //    DrawValidity();
            //}
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

        //void DrawValidity()
        //{
        //    DrawValidityGraphSo();
        //}

        //void DrawValidityGraphSo()
        //{
        //    runner.isReady = true;

        //    foreach (var item in runner.storage.Values)
        //    {
        //        GUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField(item.Name);

        //        if (DoesSoContainsValue(item.Name, item.GetValueType()))
        //        {
        //            EditorGUILayout.LabelField("    OK");
        //        }
        //        else
        //        {
        //            runner.isReady = false;
        //            EditorGUILayout.LabelField("    NOK");
        //        }

        //        GUILayout.EndHorizontal();
        //    }
        //}

        //bool DoesSoContainsValue(string name, Type valueType)
        //{
        //    var properties = runner.So.GetType().GetProperties();

        //    foreach (var property in runner.So.GetType().GetFields())
        //    {
        //        if (property.Name == name && property.FieldType == valueType)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}