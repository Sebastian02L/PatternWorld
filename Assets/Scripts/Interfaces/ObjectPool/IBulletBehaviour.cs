using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public interface IBulletBehaviour
    {
        public void OnCollisionBehaviour(Collider other, float damage, Action callback, GameObject bulletGO);
    }
}
