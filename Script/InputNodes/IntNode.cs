using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/IntInputNode")]
    public class IntNode : Leaf<int>
    {
        public int value;
    }
}