﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Graph
{
    public class DefaultGraph : NodeGraph
    {
        [SerializeField] public Blackboard blackboard;
        public GenericDicionnary gd = new GenericDicionnary();
        public bool CanRun;

        public Root root;

        public object Run(GenericDicionnary newgd = null)
        {
            if (newgd != null)
            {
                this.gd = newgd;
            }

            return root.GetValue(root.Ports.First());
        }

        public void UpdateDictionnary(Dictionary<string, Variable> newDictionnary)
        {
            MergeDictionnaries(blackboard.container, newDictionnary);
        }

        public Dictionary<string, Variable>   CompileAllBlackboard()
        {
            Dictionary<string, Variable> CompiledDictionnary =
                new Dictionary<string, Variable>();

            MergeDictionnaries(CompiledDictionnary, blackboard.container);

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