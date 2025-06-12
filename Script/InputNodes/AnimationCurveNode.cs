using UnityEngine;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Constants/AnimationCurve")]
    [NodeTint(ColorProfile.Input)]
    public class AnimationCurveNode : ConstantNode<AnimationCurve> { }
}