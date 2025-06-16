using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    public class DefaultGraph : NodeGraph
    {
        [HideInInspector] public Blackboard blackboard;
        public GraphVariables originalStorage = new GraphVariables();
        [HideInInspector] public GraphVariables runtimeStorage;
        [HideInInspector] public bool CanRun;
        [HideInInspector] public Root root;
        public virtual Type GetRootNodeType() => typeof(Root);

        public virtual object Run(GraphVariables newstorage = null)
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