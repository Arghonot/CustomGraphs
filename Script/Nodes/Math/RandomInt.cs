using UnityEngine;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Math/RandomInt")]
    [NodeTint(ColorProfile.Mathematics)]
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