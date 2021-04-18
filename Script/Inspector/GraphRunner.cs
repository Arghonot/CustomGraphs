using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class GraphRunner<T> : MonoBehaviour where T: Graph.DefaultGraph
    {
        public ScriptableObject So;
        public T graph;
        public bool isReady;

        [SerializeField]
        public GraphVariableStorage storage = new GraphVariableStorage();

        public void BuildValueDictionnary()
        {
            if (graph.blackboard == null)
            {
                graph = null;
                return;
            }
            if (storage != null)
            {
                storage.Flush();
            }
            else
            {
                storage = new GraphVariableStorage();
            }

            storage.Merge(graph.CompileAllBlackboard());
        }
    }
}