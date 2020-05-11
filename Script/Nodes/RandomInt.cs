using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/RandomInt")]
    public class RandomInt : Leaf
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Min;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Max;

        private void Awake()
        {
            AddDynamicOutput(typeof(int), ConnectionType.Multiple, TypeConstraint.Strict, "int");
        }

        public override object Run()
        {
            return (int)Random.Range(Min, Max);
        }
    }
}