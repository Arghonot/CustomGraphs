﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("MYBT/IntInputNode")]
    [NodeTint("#a8dadc")]
    public class IntNode : Node
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)]
        public int value;
    }
}