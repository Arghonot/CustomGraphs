using UnityEngine;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(CustomGraph.SubGraphMaster))]
    public class SubGraphEditor : XNodeEditor.NodeEditor
    {
        public override void OnBodyGUI()
        {
            CustomGraph.SubGraphMaster SubGraph = target as CustomGraph.SubGraphMaster;
            CustomGraph.DefaultGraph owngraph = (CustomGraph.DefaultGraph)SubGraph.graph;

            CustomGraph.DefaultGraph prevGraph = SubGraph.targetSubGraph;

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