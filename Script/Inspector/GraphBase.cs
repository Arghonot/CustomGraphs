using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    public class GraphBase : NodeGraph
    {
        [HideInInspector] public Blackboard blackboard;
        public GraphVariables originalStorage = new GraphVariables();
        [HideInInspector] public GraphVariables runtimeStorage;
        [HideInInspector] public bool CanRun;
        [HideInInspector] public Root root;
        public virtual Type GetRootNodeType() => typeof(Root);

        public virtual void Initialize()
        {
            if (blackboard == null)
            {
                var newBlackboard = AddNode<Blackboard>();
                blackboard = newBlackboard;
            }
        }
        public virtual object Run(GraphVariables newstorage = null)
        {
            runtimeStorage = newstorage;
            return root.GetValue(root.Ports.First());
        }
    }
}