using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace Graph
{
    public class SubGraphNode : NodeBase
    {
        public DefaultGraph SubGraph;

        public override object Run()
        {
            Debug.Log("DefaultGraph");

            return null;
            //TargetGraph.gd = ((DefaultGraph)graph).gd;

            //if (TargetGraph == null)
            //{
            //    return new Object();
            //}

            //return GetInputValue<DefaultGraph>("TargetGraph", this.TargetGraph);
        }
    }
}