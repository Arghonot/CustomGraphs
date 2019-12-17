using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XNode;

namespace BT.Leaves
{
    public class StopWalking : BTNode
    {
        public override BTState Run()
        {
            var agent = AIcontext.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
            }

            agent.isStopped = true;

            return BTState.Success;
        }
    }
}