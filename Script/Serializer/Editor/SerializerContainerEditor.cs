using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SerializerContainer))]
public class SerializerContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var oldgraph = ((SerializerContainer)target).graph;

        base.OnInspectorGUI();

        if (oldgraph != ((SerializerContainer)target).graph)
        {
            ((SerializerContainer)target).sbb.InitializeContent(
                ((SerializerContainer)target).graph);
        }
    }
}
