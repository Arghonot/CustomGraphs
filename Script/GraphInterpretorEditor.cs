using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GraphInterpretor))]
public class GraphInterpretorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GraphInterpretor runner = target as GraphInterpretor;

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

    void DrawGraphBlackboard(GraphInterpretor runner)
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
        GUILayout.BeginHorizontal("line");
        elem.DrawInspectorGUI();
        GUILayout.EndHorizontal();
    }
}
