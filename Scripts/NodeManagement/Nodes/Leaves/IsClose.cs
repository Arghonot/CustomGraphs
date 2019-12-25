using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    public class IsClose : BTNode
    {
        public string DistanceSource;
        public string ObjectA;
        public string ObjectB;

        public override BTState Run()
        {
            var objA = AIcontext.Get<Transform>(ObjectA);
            var objB = AIcontext.Get<Transform>(ObjectB);
            float distance;

            if (objA == null || objB == null)
            {
                return BTState.Failure;
            }

            distance = Vector3.Distance(objA.position, objB.position);

            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log("IsClose [distance]" + distance);
            }

            if (distance > AIcontext.Get<float>(DistanceSource))
            {
                return BTState.Failure;
            }

            return BTState.Success;
        }
    }
}