using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemyBulletBehaviour : ABulletBehaviour
    {
        public override void OnCollisionBehaviour(Collider other, float damage, Action callback, GameObject bulletGO)
        {
            base.OnCollisionBehaviour(other, damage, callback, bulletGO);
        }
    }
}
