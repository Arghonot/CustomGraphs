using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XNode;

namespace BT.Leaves
{
    public class GetRandomPosition : BTNode
    {
        public string AreaToUse;
        public string RandomPositionName;

        public bool UseNavmesh;
        public float WalkRadius;

        public override BTState Run()
        {
            if (UseNavmesh)
            {
                AIcontext.Set<Vector3>(
                    RandomPositionName,
                    GetRandomPositionOnNavmesh());

                return BTState.Success;
            }

            return GetRandomFromBounds();
        }

        protected BTState GetRandomFromBounds()
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

        protected Vector3 GetRandomPositionOnNavmesh()
        {
            Transform self = AIcontext.Get<Transform>("self");

             Vector3 randomDirection = self.position +
                Random.insideUnitSphere *
                WalkRadius;

            randomDirection += self.position;
            NavMeshHit hit;

            if (!NavMesh.SamplePosition(randomDirection, out hit, WalkRadius, 1))
            {
                return self.position;
            }

            Debug.Log(hit.position);

            return hit.position;
        }

        protected Vector3 GetRandom(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x , bounds.max.x),
                Random.Range(bounds.min.y , bounds.max.y),
                Random.Range(bounds.min.z , bounds.max.z));
        }
    }
}