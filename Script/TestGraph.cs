using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;


namespace BT
{
    [CreateAssetMenu]
    public class TestGraph : NodeGraph
    {
        public Blackboard blackboard;
        string blackboardName = "Blacboard";

        private void Awake()
        {
            Debug.Log("TestGraph Awake");

            blackboard = AddNode<Blackboard>();
            blackboard.name = blackboardName;

            NodeEditorWindow.RepaintAll();
        }

        public Dictionary<string, Variable>   CompileAllBlackboard()
        {
            Dictionary<string, Variable> CompiledDictionnary =
                new Dictionary<string, Variable>(blackboard.container);

            foreach (var node in nodes)
            {
                if (node.GetType() == typeof(SubGraph))
                {
                    if (((SubGraph)node).TargetGraph != null)
                    {
                        HandleSubGraph(CompiledDictionnary, (SubGraph)node);
                        Debug.Log("Double yeah");
                    }
                    Debug.Log("yeah");
                }
            }

            return CompiledDictionnary;
        }

        void HandleSubGraph(Dictionary<string, Variable> compiled, SubGraph graph)
        {
            var subgraphDictionnary = graph.TargetGraph.CompileAllBlackboard();

            foreach (var item in subgraphDictionnary)
            {
                if (!compiled.ContainsKey(item.Key) && !ContainName(compiled, item.Value.Name))
                {
                    compiled.Add(item.Key, item.Value);
                }
            }
        }

        bool ContainName(Dictionary<string, Variable> dic, string name)
        {
            foreach (var item in dic)
            {
                if (item.Value.Name == name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}