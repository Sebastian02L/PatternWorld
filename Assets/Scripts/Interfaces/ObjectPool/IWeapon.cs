using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public interface IWeapon
    {
        public event Action<int> onAmmoChange;

        public void ShootCanceled();
        public void Shoot();
        public void Reload();
        public void SetWeaponData(WeaponData weaponData);
    }
}
