using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static XNodeEditor.NodeEditor;

namespace BTEditor
{
    [CustomNodeEditor(typeof(BT.SubGraph))]
    public class SubGraphEditor : XNodeEditor.NodeEditor
    {
        public override void OnBodyGUI()
        {
            BT.SubGraph SubGraph = target as BT.SubGraph;
            BT.TestGraph owngraph = (BT.TestGraph)SubGraph.graph;

            SubGraph.TargetGraph = (BT.TestGraph)EditorGUILayout.ObjectField(
                "Sub graph ",
                SubGraph.TargetGraph,
                typeof(BT.TestGraph),
                false);

            if (SubGraph.TargetGraph == owngraph)
            {
                SubGraph.TargetGraph = null;
                Debug.LogError("[SubGraph Node Error] : Can't feed self graph to a subgraph node.");
            }

        }
    }
}