using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ReflectShieldManager : AWeapon
    {
        bool firstFrame = true;
        int shootedBullets = 0;

        protected void Start()
        {
            Started(true);
            bulletBehaviour = new ReflectBulletBehaviour(); 
        }

        protected override void Update()
        {
            base.Update();
            if (firstFrame)
            {
                ammo = 0; 
                InvokeAmmoChange();
                firstFrame = false;
            }
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0 && shootedBullets < weaponData.maxAmmo)
            {
                canShoot = false;
                IPoolableObject bullet =GetBulletFromPool();

                if (bullet != null)
                {
                    GameObject bulletGO = SetUpBullet(bullet, true);
                    Vector3 impactPoint = CalculateBulletDirection();
                    AdjustAndActivateBullet(bulletGO, impactPoint);
                    shootedBullets++;
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldShoot", 0.5f);
                    animator.SetTrigger("Shoot");
                }
            }
            else if (shootedBullets >= weaponData.maxAmmo)
            {
                Debug.Log("Cantidad maxima disparada. Recargando...");
                Reload();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Bullets")) return;
            if(!reloading) AddAmmo();
        }

        void AddAmmo()
        {
            if(ammo < weaponData.maxAmmo)
            {
                ammo++;
                InvokeAmmoChange();
                AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldAddAmmo", 0.5f);
                animator.SetTrigger("AddAmmo");
                reloadFVX.Play();
            }
        }

        public override void Reload()
        {
            if (reloading) return;
            if (shootedBullets == 0) return;
            InvokeReload();
            canShoot = false;
            reloading = true;
            shootedBullets = 0;
            AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldCooldown", 0.5f);
            animator.SetTrigger("Reload");
        }

        public override void ShootCanceled()
        {
        }
    }
}
