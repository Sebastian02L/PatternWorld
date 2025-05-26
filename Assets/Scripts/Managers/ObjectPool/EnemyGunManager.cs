using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemyGunManager : AWeapon
    {
        [SerializeField] Light gunLight;

        void SetUp()
        {
            Started(false);
            ammo = weaponData.maxAmmo;
            bulletBehaviour = new EnemyBulletBehaviour();
            shootVFX.startColor = weaponData.lightColor;
        }

        public override void SetWeaponData(WeaponData weaponData)
        {
            base.SetWeaponData(weaponData);
            gunLight.color = weaponData.lightColor;
            SetUp();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0)
            {
                gunLight.enabled = true;
                canShoot = false;
                IPoolableObject bullet = GetBulletFromPool();

                if (bullet != null)
                {
                    GameObject bulletGO = SetUpBullet(bullet, false);
                    Vector3 impactPoint = CalculateBulletDirection();
                    AdjustAndActivateBullet(bulletGO, impactPoint);
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_EnemyShoot", 0.5f);
                }
            }
            else if (ammo == 0)
            { 
                Reload();
                gunLight.enabled = false;
            }
        }

        public override void Reload()
        {
            base.Reload();
        }

        protected override Vector3 CalculateBulletDirection()
        {
            RaycastHit hit;
            if (Physics.Raycast(shootOrigin.transform.position, ((playerCamera.transform.position - new Vector3(0f, 0.362f, 0f)) - shootOrigin.transform.position), out hit, 100f, ~(1 << 8)))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        public override void ShootCanceled()
        {
        }
    }
}
