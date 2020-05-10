using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("MYBT/StringNode")]
    public class StringNode : Node
    {
        [Input]
        public string value;
    }
}