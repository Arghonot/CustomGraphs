using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/BoolNode")]
    public class BoolNode : Leaf
    {
        public bool value;

        private void Awake()
        {
            AddDynamicOutput(
                typeof(bool),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                "bool");
        }
    }
}