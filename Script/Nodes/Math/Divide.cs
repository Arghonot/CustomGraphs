using UnityEngine;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Math/Divide")]
    public class Divide : Leaf<double>
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