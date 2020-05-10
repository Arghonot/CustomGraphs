using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static XNodeEditor.NodeEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(Graph.SubGraph))]
    public class SubGraphEditor : XNodeEditor.NodeEditor
    {
        public override void OnBodyGUI()
        {
            Graph.SubGraph SubGraph = target as Graph.SubGraph;
            Graph.TestGraph owngraph = (Graph.TestGraph)SubGraph.graph;

            SubGraph.TargetGraph = (Graph.TestGraph)EditorGUILayout.ObjectField(
                "Sub graph ",
                SubGraph.TargetGraph,
                typeof(Graph.TestGraph),
                false);

            if (SubGraph.TargetGraph == owngraph)
            {
                SubGraph.TargetGraph = null;
                Debug.LogError("[SubGraph Node Error] : Can't feed self graph to a subgraph node.");
            }

        }
    }
}