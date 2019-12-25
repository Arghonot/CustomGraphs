using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Composite
{
    /// <summary>
    /// The start of a graph.
    /// The first found will be used, usually it shall be the first created.
    /// </summary>
    public class BTRoot : BTNode
    {
        public override BTState Run()
        {
            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log("+-----------------------------+");
            }

            return GetInputValue("inPort", BTState.Success);
        }
    }
}