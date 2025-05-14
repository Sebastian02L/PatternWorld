using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class FlareRifleManager : AWeapon
    {
        protected void Start()
        {
            Started(true);
            bulletBehaviour = new FlareBulletBehaviour();
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
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_FlareRifleShoot", 0.5f);
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
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_FlareRifleReload", 0.5f);
            animator.SetTrigger("Reload");
            reloadFVX.Play();
            base.Reload();
        }

        public override void ShootCanceled()
        {
        }
    }
}
