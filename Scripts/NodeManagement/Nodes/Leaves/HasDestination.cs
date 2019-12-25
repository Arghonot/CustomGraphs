using UnityEngine;
using UnityEngine.AI;
using XNode;
namespace BT.Leaves
{
    public class HasDestination : BTNode
    {
        public string DestinationPath;

        public override BTState Run()
        {
            var agent = AIcontext.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                // We have to have a NavMeshAgent on this agent
                return BTState.Failure;
                //throw new System.Exception();
            }

            return HasReachedDestination();
        }

        BTState HasReachedDestination()
        {
            var navagent = AIcontext.Get<NavMeshAgent>("agent");

            if (!navagent.hasPath)
            {
                return BTState.Failure;
            }

            float dist = Vector3.Distance(navagent.gameObject.transform.position,
                AIcontext.Get<Vector3>(DestinationPath));

            if (navagent.pathPending)
            {
                return BTState.Success;
            }

            if (navagent.pathStatus == NavMeshPathStatus.PathComplete &&
                navagent.remainingDistance == 0)
            {
                return BTState.Failure;
            }

            if (navagent.pathStatus == NavMeshPathStatus.PathPartial &&
                dist < 3f)
            {
                Debug.Log("PENDING");
                return BTState.Failure;
            }

            return BTState.Success;
        }
    }
}