using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    public class Follow : BTNode
    {
        public float RefreshRate;
        public string Target;

        private float _currentRefreshRate;

        public override BTState Run()
        {
            _currentRefreshRate += Time.deltaTime;

            if (_currentRefreshRate > RefreshRate)
            {
                _currentRefreshRate = 0f;

                return FollowTarget() ? BTState.Success : BTState.Failure;
            }

            return BTState.Failure;
        }

        bool FollowTarget()
        {
            NavMeshAgent agent = AIcontext.Get<NavMeshAgent>("agent");
            Transform target = AIcontext.Get<Transform>(Target);

            return agent.SetDestination(target.position);
        }
    }
}