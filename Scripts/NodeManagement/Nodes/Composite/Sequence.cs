using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Composite
{
    public class Sequence : BTNode
    {
        public string DebugName;

        public override BTState Run()
        {
            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log("SEQUENCE " + DebugName);
            }

            NodePort inport = GetPort("inPort");

            List<NodePort> connections = inport.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                var res = connections[i].GetOutputValue();

                if (res != null)
                {
                    BTState result = (BTState)res;

                    if (AIcontext.Get<bool>("ShallDebug"))
                    {
                        Debug.Log(DebugName + " Got : " + result);
                    }

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