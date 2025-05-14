using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnergyPistolManager : AWeapon
    {
        protected void Start()
        {
            Started(true);
            bulletBehaviour = new EnergyPistolBulletBehaviour();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0)
            {
                canShoot = false;
                IPoolableObject bullet = GetBulletFromPool();

                if (bullet != null)
                {
                    GameObject bulletGO = SetUpBullet(bullet, true);
                    Vector3 impactPoint = CalculateBulletDirection();
                    AdjustAndActivateBullet(bulletGO, impactPoint);
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_EnergyPistolShoot", 0.5f);
                    animator.SetTrigger("Shoot");
                }
            }
            else if (ammo == 0)
            { 
                Reload();
            }
        }

        public override void Reload()
        {
            if (ammo == weaponData.maxAmmo) return;
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_EnergyPistolReload", 0.5f);
            reloadFVX.Play();
            animator.SetTrigger("Reload");
            base.Reload();
        }

        public override void ShootCanceled()
        {
        }
    }
}
