using UnityEngine;

namespace CustomGraph
{
    public class GraphRunner<T> : MonoBehaviour where T: CustomGraph.DefaultGraph
    {
        public ScriptableObject So;
        public T graph;
        public bool isReady;

        [SerializeField]
        public GraphVariables storage = new GraphVariables();
    }
}