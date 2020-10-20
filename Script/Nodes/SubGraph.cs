using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace Graph
{
    public class SubGraph : Leaf<int>
    {
        public Graph.IntGraph TargetGraph;

        public override object Run()
        {
            TargetGraph.gd = ((DefaultGraph)graph).gd;
            return TargetGraph.Root.Run();
        }
    }
}