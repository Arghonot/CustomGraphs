using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Constants/AnimationCurve")]
    [NodeTint(ColorProfile.Input)]
    public class AnimationCurveNode : ConstantNode<AnimationCurve> { }
}