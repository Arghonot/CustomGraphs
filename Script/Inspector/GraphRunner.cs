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
    }
}