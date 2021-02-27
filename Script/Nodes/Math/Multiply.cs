using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XNode.Node;

namespace Graph
{
    [CreateNodeMenu("Graph/Math/Multiply")]
    public class Multiply : Leaf<double>
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Inherited)]
        public double A;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Inherited)]
        public double B;

        public override object Run()
        {
            return GetInputValue<double>("A", this.A) * GetInputValue<double>("B", this.B);
        }
    }
}