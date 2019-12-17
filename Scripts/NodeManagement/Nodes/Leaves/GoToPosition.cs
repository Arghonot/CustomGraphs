using UnityEngine;
using UnityEngine.AI;
using XNode;

namespace BT.Leaves
{
    public class GoToPosition : BTNode
    {
        public string PositionToReach;

        public override BTState Run()
        {
            var agent = AIcontext.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
            }

            agent.SetDestination(AIcontext.Get<Vector3>(PositionToReach));

            return BTState.Success;
        }
    }
}