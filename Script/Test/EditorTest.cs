using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorTest))]
public class EditorTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorTest val = (EditorTest)target;

        val.myint = EditorGUILayout.IntField(val.myint);

        if (GUILayout.Button("Print"))
        {
            Debug.Log(val.myint);
        }
    }
}

public class EditorTest : MonoBehaviour
{
    public int myint;
}
