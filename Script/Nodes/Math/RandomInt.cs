using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Math/RandomInt")]
    public class RandomInt : Leaf<int>
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Min;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Max;

        public override object Run()
        {
            return (int)Random.Range(Min, Max);
        }
    }
}