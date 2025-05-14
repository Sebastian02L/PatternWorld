using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class RefrigeratedSwordManager : AWeapon
    {
        protected void Start()
        {
            Started(true);
            bulletBehaviour = new RefrigeratedBulletBehaviour();
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
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_SwordShoot", 0.5f);
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
            if(ammo == weaponData.maxAmmo) return;
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_SwordReload", 0.5f);
            base.Reload();
            reloadFVX.Play();
            animator.SetTrigger("Reload");
        }

        public override void ShootCanceled()
        {
        }
    }
}
