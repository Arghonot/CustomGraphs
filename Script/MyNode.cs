using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/SimpleNode")]
    public class MyNode : Node
    {
        [Input]
        public int value;
    }
}