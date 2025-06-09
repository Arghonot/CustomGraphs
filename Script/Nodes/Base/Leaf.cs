using UnityEngine;

namespace Graph
{
    [NodeTint(ColorProfile.Leaf)]
    [HideInInspector]
    [HideFromNodeMenu]
    public class Leaf<T> : NodeBase
    {
        public virtual void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(typeof(T), ConnectionType.Multiple, TypeConstraint.Strict, "Output");
            }
        }
    }
}