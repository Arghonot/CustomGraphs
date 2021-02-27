using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureGenerator))]
public class SerializerContainerEditor : Editor
{
    // TODO use for real later on
    public override void OnInspectorGUI()
    {
        var oldgraph = ((TextureGenerator)target).graph;

        base.OnInspectorGUI();

        if (((TextureGenerator)target).graph != null &&
            oldgraph != ((TextureGenerator)target).graph)
        {
            Debug.Log("DIFF");
            ((TextureGenerator)target).sbb.InitializeContent(
                ((TextureGenerator)target).graph);
        }
    }
}
