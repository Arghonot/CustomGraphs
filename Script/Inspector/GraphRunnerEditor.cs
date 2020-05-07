using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GraphRunner))]
public class GraphRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GraphRunner runner = target as GraphRunner;

        if (runner == null)
        {
            return;
        }

        var TmpGraph = runner.graph;
        TmpGraph = (BT.TestGraph)EditorGUILayout.ObjectField(runner.graph, typeof(BT.TestGraph), true);

        if (TmpGraph == null)
        {
            return;
        }
        else if (TmpGraph != runner.graph)
        {
            runner.graph = TmpGraph;
            runner.BuildValueDictionnary();
        }

        DrawGraphBlackboard(runner);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawGraphBlackboard(GraphRunner runner)
    {
        if (runner.values == null)
        {
            return;
        }

        foreach (var item in runner.values)
        {
            DrawLine(item.Value);
        }
    }

    void DrawLine(Variable elem)
    {
        //Debug.Log("Elem is null ?" + (elem == null) + " Value == null ?" + (elem.Value == null));

        GUILayout.BeginHorizontal();
        elem.DrawInspectorGUI();
        GUILayout.EndHorizontal();
    }
}
