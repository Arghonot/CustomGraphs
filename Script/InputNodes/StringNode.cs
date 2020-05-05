using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/StringNode")]
    public class StringNode : Node
    {
        [Input]
        public string value;
    }
}