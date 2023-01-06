using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [NodeTint(ColorProfile.Leaf)]
    [HideInInspector]
    public class Leaf<T> : NodeBase
    {
        public virtual void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(
                    typeof(T),
                    ConnectionType.Multiple,
                    TypeConstraint.Strict,
                    "Output");
            }
        }
    }
}