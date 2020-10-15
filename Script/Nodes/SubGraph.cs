using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.StandardLeaves
{
    public class SubGraph : AILeaf
    {
        public Graph.IntGraph TargetGraph;

        public override object Run()
        {
            TargetGraph.gd = Gd;
            return TargetGraph.Root.Run();//GetValue( Root.Ports.First());
        }
    }
}