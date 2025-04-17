using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public interface IWeapon
    {
        public event Action<int> onAmmoChange;
        public void Shoot();
        public void Reload();
    }
}
