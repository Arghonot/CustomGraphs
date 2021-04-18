using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Graph
{
    public class DefaultGraph : NodeGraph
    {
        [SerializeField] public Blackboard blackboard;
        public GraphVariableStorage storage = new GraphVariableStorage();
        public bool CanRun;

        public Root root;

        public object Run(GraphVariableStorage newstorage = null)
        {
            if (newstorage != null)
            {
                this.storage = newstorage;
            }

            return root.GetValue(root.Ports.First());
        }

        public void UpdateDictionnary(GraphVariableStorage newstorage)
        {
            MergeDictionnaries(blackboard.storage, newstorage);
        }

        public GraphVariableStorage   CompileAllBlackboard()
        {
            GraphVariableStorage CompiledDictionnary = new GraphVariableStorage();

            MergeDictionnaries(CompiledDictionnary, blackboard.storage);

            foreach (var node in nodes)
            {
                if (node.GetType().IsSubclassOf(typeof(SubGraphNode)))
                {
                    if (((SubGraphNode)node).SubGraph != null)
                    {
                        MergeDictionnaries(
                            CompiledDictionnary,
                            ((SubGraphNode)node).SubGraph.CompileAllBlackboard());
                    }
                }
            }

            return CompiledDictionnary;
        }

        // for performance reason you don't want this graph's dictionnary to directly add the new values
        // because if you have 10 graph nested in each other you will end up checking the same values over and over.
        void MergeDictionnaries(GraphVariableStorage compiled, GraphVariableStorage subgraphDictionnary)
        {
            compiled.Merge(subgraphDictionnary);

            //foreach (var item in subgraphDictionnary)
            //{
            //    if (!compiled.ContainsKey(item.Key) && !ContainName(compiled, item.Value.Name))
            //    {
            //        compiled.Add(item.Key, item.Value);
            //    }
            //}
        }

        //bool ContainName(GraphVariableStorage dic, string name)
        //{
        //    foreach (var item in dic)
        //    {
        //        if (item.Value.Name == name)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public void OnDeleteVariable(string uid)
        {
            foreach (var node in nodes)
            {
                if (node.GetType() == typeof(BlackBoardVariable))
                {
                    BlackBoardVariable bbvar = node as BlackBoardVariable;

                    if (bbvar.guid == uid)
                    {
                        bbvar.UpdateNode();
                    }
                }
            }
        }
    }
}