using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Decorator
{
    public class Inverter : BTNode
    {
        public override BTState Run()
        {
            return GetInputValue("inPort", BTState.Success) == BTState.Success ?
                BTState.Failure :
                BTState.Success;
        }
    }
}
