using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnergyPistolBulletBehaviour : ABulletBehaviour
    {
        public override void OnCollisionBehaviour(Collider other, float damage, Action callback)
        {
            base.OnCollisionBehaviour(other, damage, callback);
        }
    }
}
