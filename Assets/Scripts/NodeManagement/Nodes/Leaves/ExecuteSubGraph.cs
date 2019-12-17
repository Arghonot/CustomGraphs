using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    /// <summary>
    /// This node will execute a sub node, starting by it's root.
    /// </summary>
    public class ExecuteSubGraph : BTNode
    {
        public BTGraph bTGraph;

        public override BTState Run()
        {
            if (bTGraph != null)
            {
                return bTGraph.Run(AIcontext);
            }

            return BTState.Failure;
        }
    }
}