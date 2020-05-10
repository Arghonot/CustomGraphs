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
            Graph.DefaultGraph owngraph = (Graph.DefaultGraph)SubGraph.graph;

            SubGraph.TargetGraph = (Graph.DefaultGraph)EditorGUILayout.ObjectField(
                "Sub graph ",
                SubGraph.TargetGraph,
                typeof(Graph.DefaultGraph),
                false);

            if (SubGraph.TargetGraph == owngraph)
            {
                SubGraph.TargetGraph = null;
                Debug.LogError("[SubGraph Node Error] : Can't feed self graph to a subgraph node.");
            }

        }
    }
}