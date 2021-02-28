using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/Constants/BoolNode")]
    public class BoolNode : Leaf<bool>
    {
        public bool value;
    }
}