using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Composite
{
    public class Selector : BTNode
    {
        public override BTState Run()
        {
            NodePort inport = GetPort("inPort");

            List<NodePort> connections = inport.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                if ((BTState)connections[i].GetOutputValue() == BTState.Success)
                {
                    return BTState.Success;
                }
            }

            return BTState.Failure;
        }
    }
}