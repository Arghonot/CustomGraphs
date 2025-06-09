using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Constants/StringNode")]
    public class StringNode : Leaf<string>
    {
        public string value;
    }
}