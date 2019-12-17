using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Composite
{
    public class Sequence : BTNode
    {
        public override BTState Run()
        {
            NodePort inport = GetPort("inPort");

            List<NodePort> connections = inport.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                var res = connections[i].GetOutputValue();

                if (res != null)
                {
                    BTState result = (BTState)res;
                    if (result == BTState.Failure)
                    {
                        return BTState.Failure;
                    }
                }
            }

            return BTState.Success;
        }
    }
}