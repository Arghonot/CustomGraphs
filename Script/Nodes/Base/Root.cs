
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace Graph
{
    [NodeTint("#f1faee")]
    public class Root<T> : NodeBase
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public T Input;
    }
}