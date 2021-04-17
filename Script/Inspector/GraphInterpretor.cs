using UnityEngine;

namespace Graph
{
    /// <summary>
    /// This class is interesting if you need to set variables for your 
    /// graph instance and easily share it between scenes
    /// </summary>
    [CreateAssetMenu(fileName = "GraphDatas", menuName = "Graphs/GraphData", order = 1)]
    public class GraphInterpretor : ScriptableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Graph.DefaultGraph graph;

        /// <summary>
        /// Organized as follow : GUID - Value's datas
        /// </summary>
        public GraphVariableStorage storage = new GraphVariableStorage();

        public void BuildValueDictionnary()
        {
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
