using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    public class FacePosition : BTNode
    {
        public string TransformToBeFaced;

        public override BTState Run()
        {
            Transform self = AIcontext.Get<Transform>("self");
            Transform target = AIcontext.Get<Transform>(TransformToBeFaced);
            Vector3 lookPos =
                target.position -
                self.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            self.rotation = Quaternion.Slerp(
                self.rotation, rotation,
                0.25f);

            if (isInConeOfSight(self, target))
            {
                return BTState.Success;
            }

            return BTState.Failure;
        }

        bool isInConeOfSight(Transform self, Transform target)
        {
            Vector3 directiontotarget = target.position - self.position;

            float seingvalue =
                Vector3.Dot(directiontotarget.normalized, self.forward) *
                Mathf.Rad2Deg;

            if (seingvalue - 30f > 0)
            {
                return true;
            }

            return false;
        }
    }
}