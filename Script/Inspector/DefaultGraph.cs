using System.Collections.Generic;
using UnityEngine;
using XNode;
using BT.StandardLeaves; // fix this using non sense

namespace Graph
{
    public class DefaultGraph : NodeGraph
    {
        [SerializeField] public Blackboard blackboard;
        public GenericDicionnary gd = new GenericDicionnary();
        public bool CanRun;

        public Dictionary<string, Variable>   CompileAllBlackboard()
        {
            Dictionary<string, Variable> CompiledDictionnary =
                new Dictionary<string, Variable>();

            MergeDictionnaries(CompiledDictionnary, blackboard.container);

            foreach (var node in nodes)
            {
                if (node.GetType() == typeof(SubGraph))
                {
                    if (((SubGraph)node).TargetGraph != null)
                    {
                        MergeDictionnaries(
                            CompiledDictionnary,
                            ((SubGraph)node).TargetGraph.CompileAllBlackboard());
                    }
                }
            }

            return CompiledDictionnary;
        }

        void MergeDictionnaries(Dictionary<string, Variable> compiled, Dictionary<string, Variable> subgraphDictionnary)
        {
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

        public void OnDeleteVariable(string uid)
        {
            foreach (var node in nodes)
            {
                if (node.GetType() == typeof(BlackBoardVariable))
                {
                    BlackBoardVariable bbvar = node as BlackBoardVariable;

                    if (bbvar.uid == uid)
                    {
                        bbvar.UpdateNode();
                    }
                }
            }
        }
    }
}