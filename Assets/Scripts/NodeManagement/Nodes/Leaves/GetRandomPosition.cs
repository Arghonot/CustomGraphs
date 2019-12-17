using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    public class GetRandomPosition : BTNode
    {
        public string AreaToUse;
        public string RandomPositionName;

        public override BTState Run()
        {
            var area = AIcontext.Get<Collider>(AreaToUse);

            if (area == null)
            {
                return BTState.Failure;
                // We have to have a NavMeshAgent on this agent
                //throw new System.Exception();
            }

            AIcontext.Set<Vector3>(
                RandomPositionName,
                GetRandom(area.bounds));

            return BTState.Success;
        }

        Vector3 GetRandom(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x , bounds.max.x),
                Random.Range(bounds.min.y , bounds.max.y),
                Random.Range(bounds.min.z , bounds.max.z));
        }
    }
}