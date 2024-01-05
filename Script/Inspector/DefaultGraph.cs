using System;
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
        [HideInInspector] public GraphVariableStorage runtimeStorage;
        public bool CanRun;
        public Root root;
        public virtual Type GetRootNodeType() => typeof(Root);

        public virtual object Run(GraphVariableStorage newstorage = null)
        {
            this.runtimeStorage = newstorage;

            return root.GetValue(root.Ports.First());
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