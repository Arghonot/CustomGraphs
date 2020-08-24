﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [NodeTint(ColorProfile.Leaf)]
    public class Leaf<T> : NodeBase
    {
        public void Awake()
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