using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Graph
{
    public class DefaultGraph : NodeGraph
    {
        [SerializeField] public Blackboard blackboard;
        public GraphVariableStorage originalStorage = new GraphVariableStorage();
        public GraphVariableStorage runtimeStorage;
        public bool CanRun;
        public Root root;

        public object Run(GraphVariableStorage newstorage = null)
        {
            this.runtimeStorage = newstorage;

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
                    ((SubGraphMaster)node).targetSubGraph.EncapsulateSubGraphDictionaries(CompiledDictionnary);
                }
            }

            CompiledDictionnary.Merge(originalStorage);
            originalStorage = CompiledDictionnary;
            blackboard.storage = originalStorage;
            UpdateGraphBlackBoardVariables();
        }

        private void UpdateGraphBlackBoardVariables()
        {
            foreach (Node variable in nodes)
            {
                if (variable.GetType() == typeof(BlackBoardVariable))
                {
                    // If we just changed this guid
                    if (!originalStorage.ContainsGuid(((BlackBoardVariable)variable).guid))
                    {
                        string newGUID = originalStorage.GetGUIDFromNameAndType(
                            ((BlackBoardVariable)variable).Name,
                            ((BlackBoardVariable)variable).VariableType);
                        Debug.Log("<color=green> GUID for " + ((BlackBoardVariable)variable).Name + " wasn't found, updating it from [" + ((BlackBoardVariable)variable).guid + "] to [" + newGUID + "]</color>");
                        ((BlackBoardVariable)variable).guid = newGUID;
                    }
                    else
                    {
                        Debug.Log("<color=green> GUID for " + ((BlackBoardVariable)variable).Name + " already Contained.</color>");
                    }
                }
            }
        }

        public void UpdateDictionnary(GraphVariableStorage newstorage)
        {
            MergeDictionnaries(blackboard.storage, newstorage);
            Debug.Log("UpdateDictionnary " + originalStorage.PublicGUIDsToNames.Count);
            blackboard.storage = originalStorage;
        }

        public GraphVariableStorage   CompileAllBlackboard()
        {
            GraphVariableStorage CompiledDictionnary = new GraphVariableStorage();

            foreach (var node in nodes)
            {
                if (node.GetType().IsSubclassOf(typeof(SubGraphMaster)))
                {
                    if (((SubGraphMaster)node).targetSubGraph != null)
                    {
                        MergeDictionnaries(
                            CompiledDictionnary,
                            ((SubGraphMaster)node).targetSubGraph.CompileAllBlackboard());
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
        }

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