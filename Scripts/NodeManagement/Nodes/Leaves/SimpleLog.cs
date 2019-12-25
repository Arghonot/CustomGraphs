using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    /// <summary>
    /// This node will log into unity's console.
    /// </summary>
    public class SimpleLog : BTNode
    {
        public string StringToLog;

        public BTState StateToReturn;

        public override BTState Run()
        {
            Debug.Log(StringToLog);

            if (AIcontext.Get<bool>("ShallDebug"))
            {
            }

            return StateToReturn;
        }
    }
}