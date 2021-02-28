using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/Constants/StringNode")]
    public class StringNode : Leaf<string>
    {
        public string value;
    }
}