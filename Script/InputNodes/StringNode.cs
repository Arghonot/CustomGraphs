using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/StringNode")]
    public class StringNode : Leaf
    {
        public string value;

        private void Awake()
        {
            AddDynamicOutput(
                typeof(string),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                "string");
        }
    }
}