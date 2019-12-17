using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.Leaves
{
    public class CanShoot : BTNode
    {
        float TimeSinceLastShoot;

        public override BTState Run()
        {
            bool canGunShoot = AIcontext.Get<GunBehavior>("Gun").CanShoot();

            if (canGunShoot && isInConeOfSight(
                AIcontext.Get<Transform>("self"),
                AIcontext.Get<Transform>("target")))
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