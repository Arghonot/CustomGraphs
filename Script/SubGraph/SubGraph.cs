using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/SubGraph")]
    public class SubGraph : Node
    {
        public TestGraph TargetGraph;
    }
}