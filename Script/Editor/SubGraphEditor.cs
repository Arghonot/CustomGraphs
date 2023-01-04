using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static XNodeEditor.NodeEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(Graph.SubGraphMaster))]
    public class SubGraphEditor : XNodeEditor.NodeEditor
    {
        public override void OnBodyGUI()
        {
            Graph.SubGraphMaster SubGraph = target as Graph.SubGraphMaster;
            Graph.DefaultGraph owngraph = (Graph.DefaultGraph)SubGraph.graph;

            Graph.DefaultGraph prevGraph = SubGraph.targetSubGraph;

            base.OnBodyGUI();

            if (SubGraph.targetSubGraph == owngraph)
            {
                SubGraph.targetSubGraph = null;
                Debug.LogError("[SubGraph Node Error] : Can't feed self graph to a subgraph node.");
            }
            if (SubGraph.targetSubGraph == null && prevGraph != null)
            {
                SubGraph.OnRemoveGraph();
            }
            else if (SubGraph.targetSubGraph != prevGraph)
            {
                Debug.Log("Dropped new graph");
                SubGraph.OnDropGraph();
            }
        }
    }
}