using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Decorator
{
    public class ReturnTrue : BTNode
    {
        public override BTState Run()
        {
            GetInputValue("inPort", BTState.Success);

            return BTState.Success;
        }
    }
}