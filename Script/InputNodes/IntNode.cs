using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/IntInputNode")]
    public class IntNode : Leaf
    {
        //[Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)]
        public int value;

        private void Awake()
        {
            AddDynamicOutput(
                typeof(int),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                "int");
        }
    }
}