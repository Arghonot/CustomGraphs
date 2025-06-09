using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Constants/DoubleInputNode")]
    public class DoubleNode : ConstantNode<double>
    {
    }
}