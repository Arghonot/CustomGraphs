
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace Graph
{
    public static class ColorProfile
    {
        //public const string Root = "#f1faee";
        public const string Root = "#a8dadc";
        //public const string Branch = "#457b9d";
        public const string Branch = "#48a9a6";
        //public const string Leaf = "#a8dadc";
        public const string Leaf = "#457b9d";
        public const string Blackboard = "#1d3557";
        public const string other1 = "#e63946";
        public const string Debug = "#393D3F";
    }

    [NodeTint(ColorProfile.Root)]
    public class Root<T> : NodeBase
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public T Input;
    }
}