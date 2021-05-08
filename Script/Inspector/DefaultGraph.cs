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

        public void EncapsulateSubGraphDictionaries()
        {
            GraphVariableStorage CompiledDictionnary = new GraphVariableStorage();

            EncapsulateSubGraphDictionaries(CompiledDictionnary);
        }

        public void EncapsulateSubGraphDictionaries(GraphVariableStorage CompiledDictionnary)
        {
            foreach (var node in nodes)
            {
                if (node.GetType().IsSubclassOf(typeof(SubGraphMaster)))
                {
                    ((SubGraphMaster)node).SubGraph.EncapsulateSubGraphDictionaries(CompiledDictionnary);
                }
            }

            CompiledDictionnary.Merge(storage);
            storage = CompiledDictionnary;
            blackboard.storage = storage;
        }

        public void UpdateDictionnary(GraphVariableStorage newstorage)
        {
            MergeDictionnaries(blackboard.storage, newstorage);
            Debug.Log("UpdateDictionnary " + storage.PublicGUIDsToNames.Count);
            blackboard.storage = storage;
        }

        public GraphVariableStorage   CompileAllBlackboard()
        {
            GraphVariableStorage CompiledDictionnary = new GraphVariableStorage();

            foreach (var node in nodes)
            {
                if (node.GetType().IsSubclassOf(typeof(SubGraphMaster)))
                {
                    if (((SubGraphMaster)node).SubGraph != null)
                    {
                        MergeDictionnaries(
                            CompiledDictionnary,
                            ((SubGraphMaster)node).SubGraph.CompileAllBlackboard());
                    }
                }
            }

            MergeDictionnaries(CompiledDictionnary, blackboard.storage);
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