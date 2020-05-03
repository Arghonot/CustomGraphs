using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//public class Pair<T1, T2>
//{
//    public T1 Item1 { get; set; }
//    public T2 Item2 { get; set; }

//    public Pair(T1 val1, T2 val2)
//    {
//        Item1 = val1;
//        Item2 = val2;
//    }
//}

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
        GUILayout.BeginHorizontal("line");

        elem.DrawInspectorGUI();

        //EditorGUILayout.LabelField(elem.ValueName);

        //switch (elem.ValueType)
        //{
        //    case BT.BlackBoardType.Int:
        //        Debug.Log((int)elem.value);

        //        elem.value = EditorGUILayout.IntField((int)elem.value);
        //        Debug.Log((int)elem.value);

        //        break;
        //    case BT.BlackBoardType.Bool:
        //        Debug.Log("Bool");

        //        elem.value = (object)EditorGUILayout.Toggle((bool)elem.value);

        //        break;
        //    case BT.BlackBoardType.String:
        //        Debug.Log("String");

        //        elem.value = (object)EditorGUILayout.TextField((string)elem.value);

        //        break;
        //    default:
        //        break;
        //}

        GUILayout.EndHorizontal();
    }
}
