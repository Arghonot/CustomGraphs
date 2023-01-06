using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Graph
{
    public class ConstantNode<T> : Leaf<T>
    {
        public T value;

        public override object Run()
        {
            Debug.Log("GetValue " + value);
            return value;
        }
    }
}