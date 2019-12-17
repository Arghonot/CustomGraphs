using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Decorator
{
    /// <summary>
    /// This node will execute a son and always return false.
    /// </summary>
    public class ReturnFalse : BTNode
    {
        public override BTState Run()
        {
            NodePort inport = GetPort("inPort");

            List<NodePort> connections = inport.GetConnections();

            if (connections != null)
            {
                GetInputValue("inPort", BTState.Success);
            }

            return BTState.Failure;
        }
    }
}