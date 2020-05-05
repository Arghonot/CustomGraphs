using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/BoolNode")]
    public class BoolNode : Node
    {
        [Input]
        public bool value;
    }
}